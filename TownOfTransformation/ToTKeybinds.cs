using Rewired;
using MiraAPI.Keybinds;

namespace TownOfTransformation;

[RegisterCustomKeybinds]
public static class ToTKeybinds
{

    /// <summary>
    /// Gets the keybind for transforming.
    /// </summary>
    public static MiraKeybind Transform { get; } = new("Transform", KeyboardKeyCode.T);


}