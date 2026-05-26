using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.LocalSettings;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfTransformation.Assets;
using TownOfTransformation.Buttons.Neutral;
using TownOfTransformation.Options.Roles.Neutral;
using TownOfTransformation.Buttons.Impostor;
using TownOfTransformation.Options.Roles.Impostor;
using TownOfTransformation.Roles.Impostor;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;


using MiraAPI.Events;

using MiraAPI.Modifiers;
using MiraAPI.Networking;

using Reactor.Networking.Attributes;

using TownOfUs.Buttons.Crewmate;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Events.Crewmate;
using TownOfUs.Events.TouEvents;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game.Universal;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules;
using TownOfUs.Utilities.Appearances;
using AsmResolver.IO;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla;
using MiraAPI.Patches;
using MiraAPI.Events.Vanilla.Player;
using UnityEngine;
using UnityEngine.UI;
using TownOfTransformation.CustomMonoBehaviours;
using TownOfTransformation.Modifiers;

namespace TownOfTransformation.Roles.Neutral;

public sealed class RichGuyRole(IntPtr cppPtr)
    : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable
{
    
    [HideFromIl2Cpp] public PlayerVoteArea? ExtraVoteButton { get; private set; }
    public string LocaleKey => "RichGuy";
    public string RoleName => TouLocale.Get($"ExampleRole{LocaleKey}", "Rich Guy");
    public string RoleDescription => TouLocale.GetParsed($"ExampleRole{LocaleKey}IntroBlurb", "Do your tasks to get money!");
    public string RoleLongDescription => TouLocale.GetParsed($"ExampleRole{LocaleKey}TabDescription", "You have {money} money.").Replace("{money}",Money.ToString());
    public float Money { get; set; } = 0f;
    private int PrevComp = 0;


    public string GetAdvancedDescription()
    {
        return
            TouLocale.GetParsed($"ExampleRole{LocaleKey}WikiDescription") +
            MiscUtils.AppendOptionsText(GetType());
    }


    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new(TouLocale.GetParsed($"ExampleRole{LocaleKey}Explode", "Explode"),
                    TouLocale.GetParsed($"ExampleRole{LocaleKey}ExplodeWikiDescription"),
                    NeutAssets.SentinelExplodeSprite),
            };
        }
    }

    public Color RoleColor => TouExampleColors.Fortegreen;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralOutlier;

    public GameObject shopui;
    public float ExtraLifePrice { get; set; } = OptionGroupSingleton<RichGuyOptions>.Instance.InitialLifePrice;
    public int ExtraLivesUsed { get; set; }
    public float GoldifyPrice { get; set; } = OptionGroupSingleton<RichGuyOptions>.Instance.InitialGoldifyPrice;
    public int GoldifiesUsed { get; set; }
    public float RevealerPrice { get; set; } = OptionGroupSingleton<RichGuyOptions>.Instance.InitialRevealPrice;
    public int RevealsUsed { get; set; }
    public float ZoomoutPrice { get; set; } = OptionGroupSingleton<RichGuyOptions>.Instance.InitialZoomoutPrice;
    public int ZoomoutsUsed { get; set; }
    public float ExtraVotePrice { get; set; } = OptionGroupSingleton<RichGuyOptions>.Instance.InitialExtraVotePrice;

    public CustomRoleConfiguration Configuration => new(this)
    {
        CanUseVent = false,
        UseVanillaKillButton = false,
        IntroSound = TouAudio.GlitchSound,
        Icon = RoleIcons.Sentinel,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };



    public void Reload()
    {
        Initialize(Player);
    }

    public void RevealPurchase()
    {
        PlayerControl.LocalPlayer.NetTransform.Halt();

        if (Minigame.Instance)
        {
            return;
        }

        shopui.SetActive(false);
        var player1Menu = CustomPlayerMenu.Create();
        player1Menu.transform.FindChild("PhoneUI").GetChild(0).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
        player1Menu.transform.FindChild("PhoneUI").GetChild(1).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;

        player1Menu.Begin(
            plr => plr != null && plr != PlayerControl.LocalPlayer && !plr.Data.IsDead,
            plr =>
            {
                player1Menu.Close();

                if (plr == null)
                {
                    return;
                }
                Reveal(plr);
                
            }
        );
        foreach (var panel in player1Menu.potentialVictims)
        {
            panel.PlayerIcon.cosmetics.SetPhantomRoleAlpha(1f);
            if (panel.NameText.text != PlayerControl.LocalPlayer.Data.PlayerName)
            {
                panel.NameText.color = Color.white;
            }
        }
    }

    public void Reveal(PlayerControl target)
    {
        if (target != null)
        {
        Helpers.CreateAndShowNotification(
            TouLocale.GetParsed("ToTRoleRichGuyRevealNotif", "{target} is a {role}!").Replace("{target}",target.name).Replace("{role}",Helpers.GetRoleName(target.Data.Role)),
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
        Money -= RevealerPrice;
        RevealerPrice += OptionGroupSingleton<RichGuyOptions>.Instance.RevealPriceIncrease;
        RevealsUsed += 1;
        }
    }

    public void RevealPurchaseFailed(int reason)
    {
        shopui.SetActive(false);
        if (reason == 1)
        {
        Helpers.CreateAndShowNotification(
            TouLocale.GetParsed("ToTRoleRichGuyRevealNotif", "You've reached the max number of reveal uses!"),
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
        } else if (reason == 2)
        {
        Helpers.CreateAndShowNotification(
            TouLocale.GetParsed("ToTRoleRichGuyRevealNotif", "You don't have enough money to reveal!"),
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
        }
    }

        public void GoldifyPurchase()
    {
        PlayerControl.LocalPlayer.NetTransform.Halt();

        if (Minigame.Instance)
        {
            return;
        }

        shopui.SetActive(false);
        var player1Menu = CustomPlayerMenu.Create();
        player1Menu.transform.FindChild("PhoneUI").GetChild(0).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
        player1Menu.transform.FindChild("PhoneUI").GetChild(1).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;

        player1Menu.Begin(
            plr => plr != null && plr != PlayerControl.LocalPlayer && !plr.Data.IsDead,
            plr =>
            {
                player1Menu.Close();

                if (plr == null)
                {
                    return;
                }
                Goldify(plr);
                
            }
        );
        foreach (var panel in player1Menu.potentialVictims)
        {
            panel.PlayerIcon.cosmetics.SetPhantomRoleAlpha(1f);
            if (panel.NameText.text != PlayerControl.LocalPlayer.Data.PlayerName)
            {
                panel.NameText.color = Color.white;
            }
        }
    }

    public void Goldify(PlayerControl target)
    {
        if (target != null && target != PlayerControl.LocalPlayer)
        {
        target.RpcAddModifier<GoldifiedModifier>();
        Helpers.CreateAndShowNotification(
            TouLocale.GetParsed("ToTRoleRichGuyRevealNotif", "{target} has been goldified!").Replace("{target}",target.name),
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
        Money -= GoldifyPrice;
        GoldifyPrice += OptionGroupSingleton<RichGuyOptions>.Instance.GoldifyPriceIncrease;
        GoldifiesUsed += 1;
        }
    }

    public void GoldifyPurchaseFailed(int reason)
    {
        shopui.SetActive(false);
        if (reason == 1)
        {
        Helpers.CreateAndShowNotification(
            TouLocale.GetParsed("ToTRoleRichGuyGoldifyFailed1Notif", "You've reached the max number of goldify uses!"),
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
        } else if (reason == 2)
        {
        Helpers.CreateAndShowNotification(
            TouLocale.GetParsed("ToTRoleRichGuyGoldifyFailed2Notif", "You don't have enough money to goldify!"),
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
        }
    }      

    public void ZoomoutPurchase()
    {
        shopui.SetActive(false);
        Money -= ZoomoutPrice;
        ZoomoutPrice += OptionGroupSingleton<RichGuyOptions>.Instance.ZoomoutPriceIncrease;
        ZoomoutsUsed += 1;
        Zoom();
    }

    public void ZoomPurchaseFailed(int reason)
    {
        shopui.SetActive(false);
        if (reason == 1)
        {
        Helpers.CreateAndShowNotification(
            TouLocale.GetParsed("ToTRoleRichGuyZoomoutFailed1Notif", "You've reached the max number of zoomouts!"),
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
        } else if (reason == 2)
        {
        Helpers.CreateAndShowNotification(
            TouLocale.GetParsed("ToTRoleRichGuyZoomoutFailed2Notif", "You don't have enough money to zoom out!"),
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
        }
    }

    public void Zoom()
    {
        TownOfUs.Patches.HudManagerPatches.AdjustCameraSize(3f + (ZoomoutsUsed / 2f));
        //if (Camera.main != null)
        //    Camera.main.orthographicSize = 3f + (ZoomoutsUsed/2f);
    }

    public bool WinConditionMet()
    {
        return false;
        // change this
    }

    public void OffsetButtons()
    {
        var canVent = false;
        var transform = CustomButtonSingleton<RichGuyShopButton>.Instance;
        
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(transform, !canVent));
        
    }

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        if (Player.AmOwner)
        {
            OffsetButtons();
            HudManager.Instance.ImpostorVentButton.buttonLabelText.SetOutlineColor(TouExampleColors.Fortegreen);

        }

        shopui = UnityEngine.Object.Instantiate(NormalAssets.RichGuyShopUI.LoadAsset());
        shopui.transform.localPosition = new Vector3(0, 0, 10);
        shopui.SetActive(false);
        var shop = shopui.transform.FindChild("Shop");
        var revealer = shop.transform.FindChild("Revealer");
        var revealerprice = revealer.transform.FindChild("Purchase");
        Button revealertext = revealerprice.GetComponent<Button>();
        revealertext.onClick.AddListener(new System.Action(RichGuyPurchases.HandleRevealClick));
        var goldifier = shop.transform.FindChild("Goldify");
        var goldifierprice = goldifier.transform.FindChild("Purchase");
        Button goldifiertext = goldifierprice.GetComponent<Button>();
        goldifiertext.onClick.AddListener(new System.Action(RichGuyPurchases.HandleGoldifyClick));
        var zoomout = shop.transform.FindChild("ZoomOut");
        var zoomoutprice = zoomout.transform.FindChild("Purchase");
        Button zoomouttext = zoomoutprice.GetComponent<Button>();
        zoomouttext.onClick.AddListener(new System.Action(RichGuyPurchases.HandleZoomoutClick));

    }
  

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        if (Player.AmOwner)
        {
            HudManager.Instance.ImpostorVentButton.graphic.sprite = TouAssets.VentSprite.LoadAsset();
            HudManager.Instance.ImpostorVentButton.buttonLabelText.SetOutlineColor(TownOfUsColors.Impostor);

        }
    }




    [RegisterEvent]
    public static void OnTaskComplete(CompleteTaskEvent e)
    {
        var p = e.Player;
        var Role = p.Data.Role as RichGuyRole;
        Role.Money = Role.Money + OptionGroupSingleton<RichGuyOptions>.Instance.MoneyPerTask;
    }
    public override bool DidWin(GameOverReason gameOverReason)
    {
        return WinConditionMet();
    }


}
