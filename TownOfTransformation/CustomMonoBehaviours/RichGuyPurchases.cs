using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfTransformation.Assets;
using TownOfTransformation.Options.Roles.Impostor;
using TownOfTransformation.Roles.Impostor;
using TownOfTransformation.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;
using MiraAPI.Utilities;
using MiraAPI.Modifiers;
using TownOfTransformation.Modifiers;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Crewmate;
using TownOfTransformation.Modules;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Events;
using MiraAPI.LocalSettings;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using TownOfTransformation.Buttons.Impostor;
using TownOfUs;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Events.Crewmate;
using TownOfUs.Events.TouEvents;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game.Universal;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Impostor;
using TownOfUs.Roles.Neutral;
using HarmonyLib;
using MiraAPI.Modifiers.Types;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Universal;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Events;
using JetBrains.Annotations;
using Il2CppMono.Security.Authenticode;
using System.Collections;
using TownOfUs.Utilities.Appearances;
using Reactor.Utilities.Extensions;
using TownOfUs.Networking;
using TownOfUs.Options.Roles.Impostor;
using TownOfUs.Options;
using TownOfUs.Patches;
using TownOfTransformation.Options.Roles.Neutral;
using UnityEngine.UI;


namespace TownOfTransformation.CustomMonoBehaviours;
public class RichGuyPurchases : MonoBehaviour
{
    public void OnLifePurchase(PlayerControl richguy)
    {
        //todo
    }

    public void OnGoldifyPurchase(PlayerControl richguy)
    {
        //todo
    }

    public static void OnRevealPurchase(PlayerControl richguy)
    {
        var role = richguy.Data.Role as RichGuyRole;
        if (role.RevealsUsed < OptionGroupSingleton<RichGuyOptions>.Instance.MaxRevealUses && role.Money >= role.RevealerPrice)
        {
        role.RevealPurchase();
        role.Money -= role.RevealerPrice;
        role.RevealerPrice += OptionGroupSingleton<RichGuyOptions>.Instance.RevealPriceIncrease;
        } else
        {
            role.RevealPurchaseFailed(1);
        }
    }

    public void OnZoomoutPurchase(PlayerControl richguy)
    {
        //todo
    }

    public static void RichGuyInit(PlayerControl richguy)
    {
        if (richguy.Data.Role is not RichGuyRole role) return;
        
        var shop = role.shopui.transform.FindChild("Shop");
        var revealer = shop.transform.FindChild("Revealer");
        var revealerprice = revealer.transform.FindChild("Purchase");
        Button revealertext = revealerprice.GetComponent<Button>();
        revealertext.onClick.AddListener(new System.Action(HandleRevealClick));
    }

    public static void HandleRevealClick()
    {
        OnRevealPurchase(PlayerControl.LocalPlayer);
    }
}