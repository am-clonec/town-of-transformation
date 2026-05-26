using TownOfTransformation.Assets;
using UnityEngine;
using MiraAPI.Events;
using MiraAPI.Modifiers.Types;
using MiraAPI.Events.Vanilla.Gameplay;

namespace TownOfTransformation.Modifiers;

public sealed class GoldifiedModifier() : TimedModifier
{
    public override string ModifierName => "Goldified";
    public override bool HideOnUi => true;
    public override float Duration => 15f;
    public override bool AutoStart => true;
    public override bool ShowInFreeplay => true;

    private AnimationClip ogRun;
    
    private AnimationClip ogIdle;

    private bool ogCosmetic;
    private bool ogMovable;
    public override void OnActivate()
    {
        ogIdle = Player.MyPhysics.Animations.group.IdleAnim;
        ogRun = Player.MyPhysics.Animations.group.RunAnim;
        ogCosmetic = Player.cosmetics.gameObject.active;
        ogMovable = Player.moveable;
        Player.MyPhysics.Animations.group.RunAnim = NormalAssets.StatueAnimation.LoadAsset();
        Player.MyPhysics.Animations.group.IdleAnim = NormalAssets.StatueAnimation.LoadAsset();
        Player.MyPhysics.Animations.PlayIdleAnimation();
        Player.cosmetics.gameObject.SetActive(false);
        Player.moveable = false;
    }
    public override void OnDeactivate()
    {
        Player.MyPhysics.Animations.group.RunAnim = ogRun;
        Player.MyPhysics.Animations.group.IdleAnim = ogIdle;
        Player.MyPhysics.Animations.PlayIdleAnimation();
        Player.cosmetics.gameObject.SetActive(ogCosmetic);
        Player.moveable = ogMovable;
    }

    [RegisterEvent(10)]
    public void BeforeMurderEventHandler(BeforeMurderEvent anevent)
    {
        anevent.Cancel();
    }



}