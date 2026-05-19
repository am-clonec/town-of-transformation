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
using Epic.OnlineServices.RTC;
using TownOfTransformation.Options.Modifiers.NeutImp;
using Unity;
using UnityEditor;
using UnityEngine;
using UnityEngineInternal;
using TMPro;

namespace TownOfTransformation.Modifiers;

public sealed class PoopedModifier() : TimedModifier, IVisualAppearance
{
    public override string ModifierName => "pooped";
    public override bool HideOnUi => false;
    public bool VisualPriority => true;
    public override float Duration => OptionGroupSingleton<SkibidiToiletOptions>.Instance.PoopDuration;
    private SpriteRenderer overlay;

    public VisualAppearance GetVisualAppearance()
    {
        var nameColor = new Color(0.5019607843f, 0.3450980392f, 0.1764705882f, 1f);
        var hatId = "hat_Chocolate";
        var skinId = "skin_None";
        var visorId = "visor_None";
        var colorId = 9;
        var name = "stinky";

        
        
            
        


        return new VisualAppearance(Player.GetDefaultModifiedAppearance(), TownOfUsAppearances.Swooper)
        {
            HatId = hatId,
            SkinId = skinId,
            VisorId = visorId,
            PlayerName = name,
            PetId = "pet_EmptyPet",
            ColorId = colorId,
            NameColor = nameColor,
            ColorBlindTextColor = Color.clear,

            /*PlayerMaterialColor = new Color(1f, 0.8392156863f, 0.9254901961f, 1f),
            PlayerMaterialBackColor = new Color(0.8705882353f, 0.5725490196f, 0.7019607843f, 1f), */
            
            
            
        };
    }

    public override void OnActivate()
    {
        Player.RawSetAppearance(this);
        if (Player.AmOwner)
        {
        overlay = UnityEngine.Object.Instantiate(HudManager.Instance.FullScreen, HudManager.Instance.transform);
        overlay.transform.localPosition = new Vector3(0, 0, 10);
        overlay.gameObject.SetActive(true);
        HudManager.Instance.StartCoroutine(Effects.ColorFade(overlay, Color.clear, Color.white, 0.2f));
        overlay.sprite = NormalAssets.Poop.LoadAsset();
        }
    }

    public override void OnDeactivate()
    {
        Player.ResetAppearance();
        if (Player == PlayerControl.LocalPlayer)
        {
        overlay.Destroy();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (VanillaSystemCheckPatches.ShroomSabotageSystem && VanillaSystemCheckPatches.ShroomSabotageSystem.IsActive)
        {
            Player.RawSetAppearance(this);

        }
    }
}