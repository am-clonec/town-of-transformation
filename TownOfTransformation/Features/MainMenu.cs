// here too, this wouldn't have existed without Pix!

using UnityEngine;

namespace TownOfTransformation.Features;

public static class ReworkedMainMenu
{
    public static void SetUp(MainMenuManager menu)
    {
        menu.transform.FindChild("MainUI/AspectScaler/BackgroundTexture").GetComponent<SpriteRenderer>().color = new(0, 0.5f, 0.8f, 1);
        foreach (var btn in menu.GetComponentsInChildren<PassiveButton>())
        {
            var r = 1.3f;
            var g = 1.5f;
            var b = 1;
            btn.inactiveSprites.GetComponent<SpriteRenderer>().material.color = new(r, g, b, 1);
            btn.activeSprites.GetComponent<SpriteRenderer>().material.color = new(r + 0.5f, g + 0.5f, b + 0.5f, 1);
        }

        menu.mainButtons[0].transform.parent.parent.GetComponent<SpriteRenderer>().material.color = new(0f, 1.2f, 0.9f, 1);
    }
}