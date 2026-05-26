using TownOfTransformation.Assets;
using UnityEngine;
using MiraAPI.Events;
using MiraAPI.Modifiers.Types;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using TownOfTransformation.Modifiers;
using TownOfUs.Utilities;
using MiraAPI.Hud;
using MiraAPI.Utilities;
using TownOfUs.Buttons;
using TownOfUs.Modifiers.Game.Crewmate;
using MiraAPI.Events.Vanilla.Meeting;
using TownOfTransformation.Roles.Neutral;
using MiraAPI.Networking;

namespace TownOfTransformation.Events;

public static class RichGuyEvents
{
/*    [RegisterEvent]
    public static void EndMeetingEventHandler(EndMeetingEvent @event)
    {
        if (PlayerControl.LocalPlayer.Data.Role is not RichGuyRole role) return;
        role.Zoom();
    }*/
    [RegisterEvent]
    public static void BeforeMurderEventHandler(BeforeMurderEvent @event)
    {
        if (@event.Target.HasModifier<RichGuyTransformedModifier>())
        {
            @event.Cancel();
            CustomMurderRpc.RpcCustomMurder(source:@event.Target,target:@event.Source,resetKillTimer:false,createDeadBody:false,teleportMurderer:false,showKillAnim:false,playKillSound:false);
        }
    }
}