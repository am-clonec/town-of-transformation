using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TMPro;
using TownOfTransformation.Assets;
using TownOfTransformation.Options.Roles.Neutral;
using TownOfTransformation.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Patches;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfTransformation.Buttons.Neutral;

public sealed class RichGuyZoomButton : TownOfUsRoleButton<RichGuyRole>
{
    public override string Name => "Zoom`Out";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TouExampleColors.Fortegreen;
    public override float Cooldown => 0.1f;
    public override LoadableAsset<Sprite> Sprite => NeutAssets.SentinelKillSprite;
    public bool Zoomed { get; set; } = false;

    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(this, false));
    }


    


    public override bool CanUse()
    {

        if (HudManager.Instance.Chat.IsOpenOrOpening || MeetingHud.Instance)
        {
            return false;
        }
        return base.CanUse() && Timer <= 0f && Role.ZoomoutsUsed > 0;
    }

    protected override void OnClick()
    {
        if (Zoomed)
        {
            Role.ZoomReset();
            OverrideName("Zoom Out");
            Zoomed = false;
            
        } else
        {
            Role.Zoom();
            OverrideName("Zoom In");
            Zoomed = true;
        }
    }
}