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


namespace TownOfTransformation.Roles.Neutral;

public sealed class RichGuyRole(IntPtr cppPtr)
    : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable
{
    
    
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

    public CustomRoleConfiguration Configuration => new(this)
    {
        CanUseVent = false,
        IntroSound = TouAudio.GlitchSound,
        Icon = RoleIcons.Sentinel,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };



    public void Reload()
    {
        Initialize(Player);
    }

    

    public bool WinConditionMet()
    {
        var glitchCount = CustomRoleUtils.GetActiveRolesOfType<FortegreenRole>().Count(x => !x.Player.HasDied());

        if (MiscUtils.KillersAliveCount > glitchCount)
        {
            return false;
        }

        return glitchCount >= Helpers.GetAlivePlayers().Count - glitchCount;
    }

    public void OffsetButtons()
    {
        var canVent = false;
        var transform = CustomButtonSingleton<FortegreenTransformButton>.Instance;
        var ignite = CustomButtonSingleton<FortegreenKillButton>.Instance;
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(transform, !canVent));
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(ignite, !canVent));
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


    private static void GetTaskCounts(PlayerControl player, out int completed, out int total)
    {
        completed = 0;
        total = 0;

        if (player == null || player.Data == null)
        {
            return;
        }

        if (player.myTasks != null && player.myTasks.Count > 0)
        {
            var tasks = player.myTasks.ToArray().Where(x => !PlayerTask.TaskIsEmergency(x) && !x.TryCast<ImportantTextTask>());
            foreach (var t in tasks)
            {
                total++;
                var taskInfo = player.Data.FindTaskById(t.Id);
                var isComplete = taskInfo != null ? taskInfo.Complete : t.IsComplete;
                if (isComplete)
                {
                    completed++;
                }
            }

            return;
        }

        foreach (var info in player.Data.Tasks)
        {
            total++;
            if (info.Complete)
            {
                completed++;
            }
        }
    }
    
    public void CheckTaskCompleted()
    {
        GetTaskCounts(Player, out int CurComp, out int CurTotal);
        if (CurComp > PrevComp)
        {
            Money = Money + OptionGroupSingleton<RichGuyOptions>.Instance.MoneyPerTask;
        }
        PrevComp = CurComp;
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
