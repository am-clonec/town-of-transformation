using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
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

public sealed class RichGuyShopButton : TownOfUsRoleButton<RichGuyRole>
{
    public override string Name => "Shop";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TouExampleColors.Fortegreen;
    public override float Cooldown => 0.1f;
    public override LoadableAsset<Sprite> Sprite => NeutAssets.SentinelKillSprite;
    public bool ShopOpen { get; set; } = false;
    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(this, false));
    }


    


    public override bool CanUse()
    {
        return base.CanUse() && Timer <= 0f;
    }

    protected override void OnClick()
    {
        if (!Role.shopui.active)
        {
            Role.shopui.SetActive(true);
            var mon = Role.shopui.transform.FindChild("Money");
            var montext = mon.transform.FindChild
        } else
        {
            Role.shopui.SetActive(false);
        }
    }
}