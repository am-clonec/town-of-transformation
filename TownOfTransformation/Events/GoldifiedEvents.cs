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


namespace TownOfTransformation.Events;

public static class GoldifiedEvents
{
    [RegisterEvent]
    public static void BeforeMurderEventHandler(BeforeMurderEvent @event)
    {
        var source = @event.Source;
        var target = @event.Target;
        InvulnerabilityStuff(@event, source, target);
    }

    public static void InvulnerabilityStuff(MiraCancelableEvent miraEvent, PlayerControl source, PlayerControl target)
    {
        if (MeetingHud.Instance || ExileController.Instance)
        {
            return;
        }

        if (!target.HasModifier<GoldifiedModifier>() || target == source || !source.AmOwner)
        {
            return;
        }

        
        
        miraEvent.Cancel();  
        
        
        var normalcd = source.GetKillCooldown();
        source.SetKillTimer(normalcd * 2f);
        
    }

}