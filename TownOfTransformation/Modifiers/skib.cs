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
using TMPro;

namespace TownOfTransformation.Modifiers;

public sealed class skib() : BaseModifier, IVisualAppearance
{
    public override string ModifierName => "skib test";
    public override bool HideOnUi => false;

    private AnimationClip ogRun;
    
    private AnimationClip ogIdle;
    private Vector3 ogSize;
    private bool layer;
    private string ogColorblindName;
    private string ogName;


    public VisualAppearance GetVisualAppearance()
    {
        var name = Player.GetDefaultModifiedAppearance().PlayerName;
        var pet = Player.GetDefaultModifiedAppearance().PetId;
       
        
        name = "Skibidi";
        pet = "pet_EmptyPet";
        
        return new VisualAppearance(Player.GetDefaultModifiedAppearance(), TownOfUsAppearances.Swooper)
        {
            PlayerName = name,
            PetId = pet,
        };
    }

    public override void OnActivate()
    {
        Player.RawSetAppearance(this);
        ogIdle = Player.MyPhysics.Animations.group.IdleAnim;
        ogRun = Player.MyPhysics.Animations.group.RunAnim;
        layer = Player.cosmetics.gameObject.active;
        Player.MyPhysics.Animations.group.RunAnim = NormalAssets.SkibidiWalkAnimation.LoadAsset();
        Player.MyPhysics.Animations.group.IdleAnim = NormalAssets.SkibidiIdleAnimation.LoadAsset();
        Player.MyPhysics.Animations.PlayIdleAnimation();
        Player.cosmetics.gameObject.SetActive(false);
        
        Player.MyPhysics.Animations.group.SpriteAnimator.GetComponent<SpriteRenderer>().material =
            new(Shader.Find("Sprites/Default"));
        /*ogSize = Player.Collider.transform.localScale;
        Player.Collider.transform.localScale = new Vector3(1.5f, 1.5f, 1f);*/

        
        
        Transform Names = Player.gameObject.transform.Find("Names");
        Transform ColorblindName = Names.transform.Find("ColorblindName_TMP");
        
        ogColorblindName = ColorblindName.GetComponent<TextMeshPro>().text;
       
        ColorblindName.GetComponent<TextMeshPro>().text = "Skibidi";
    }

    public override void OnDeactivate()
    {
        Player.ResetAppearance();
        Player.MyPhysics.Animations.group.RunAnim = ogRun;
        Player.MyPhysics.Animations.group.IdleAnim = ogIdle;
        
        Player.cosmetics.gameObject.SetActive(layer);
        Player.MyPhysics.Animations.PlayIdleAnimation();
        
        SpriteRenderer rend = Player.MyPhysics.Animations.group.SpriteAnimator.GetComponent<SpriteRenderer>();
        rend.material = new Material(Shader.Find("Unlit/PlayerShader"));
        PlayerMaterial.SetColors(Player.cosmetics.ColorId, rend);

                Transform Names = Player.gameObject.transform.Find("Names");
        Transform ColorblindName = Names.transform.Find("ColorblindName_TMP");
        
        
       
        ColorblindName.GetComponent<TextMeshPro>().text = ogColorblindName;
        /*Player.Collider.transform.localScale = ogSize;*/
        
    }
}