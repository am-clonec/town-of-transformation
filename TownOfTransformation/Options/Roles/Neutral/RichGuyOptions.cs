using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfTransformation.Roles.Neutral;
using TownOfUs.Modules.Localization;

namespace TownOfTransformation.Options.Roles.Neutral;

public sealed class RichGuyOptions : AbstractOptionGroup<RichGuyRole>
{
    public override string GroupName => TouLocale.Get("RichGuy", "Rich Guy");
    [ModdedNumberOption("Money per task", 1f, 20f, 0.5f, MiraNumberSuffixes.None)]
    public float MoneyPerTask { get; set; } = 5f;
    [ModdedNumberOption("Initial Life Price", 1f, 10f, 0.5f, MiraNumberSuffixes.None)]
    public float InitialLifePrice { get; set; } = 3f;
    [ModdedNumberOption("Life Price Increase", 0f, 5f, 0.5f, MiraNumberSuffixes.None)]
    public float LifePriceIncrease { get; set; } = 1f;
    [ModdedNumberOption("Maximum Extra Lives", 0, 5, 1, MiraNumberSuffixes.None, zeroInfinity:true)]
    public int MaxExtraLives { get; set; } = 3;
    [ModdedNumberOption("Initial Goldify Price", 1f, 10f, 0.5f, MiraNumberSuffixes.None)]
    public float InitialGoldifyPrice { get; set; } = 2f;
    [ModdedNumberOption("Goldify Price Increase", 1f, 5f, 0.5f, MiraNumberSuffixes.None)]
    public float GoldifyPriceIncrease { get; set; } = 1f;
    [ModdedNumberOption("Maximum Goldify Uses", 0, 10, 1, MiraNumberSuffixes.None, zeroInfinity:true)]
    public int MaxGoldifyUses { get; set; } = 0;
    [ModdedNumberOption("Initial Reveal Price", 1f, 10f, 0.5f, MiraNumberSuffixes.None)]
    public float InitialRevealPrice { get; set; } = 3f;
    [ModdedNumberOption("Reveal Price Increase", 1f, 5f, 0.5f, MiraNumberSuffixes.None)]
    public float RevealPriceIncrease { get; set; } = 2f;
    [ModdedNumberOption("Maximum Reveal Uses", 0, 10, 1, MiraNumberSuffixes.None, zeroInfinity:true)]
    public int MaxRevealUses { get; set; } = 3;
    [ModdedNumberOption("Initial Zoomout Price", 1f, 10f, 0.5f, MiraNumberSuffixes.None)]
    public float InitialZoomoutPrice { get; set; } = 3f;
    [ModdedNumberOption("Zoomout Price Increase", 1f, 5f, 0.5f, MiraNumberSuffixes.None)]
    public float ZoomoutPriceIncrease { get; set; } = 1f;
    [ModdedNumberOption("Maximum Zoomout Uses", 1, 10, 1, MiraNumberSuffixes.None)]
    public int MaxZoomoutUses { get; set; } = 3;
    [ModdedNumberOption("Initial Extra Vote Price", 1f, 10f, 0.5f, MiraNumberSuffixes.None)]
    public float InitialExtraVotePrice { get; set; } = 1f;
    [ModdedNumberOption("Extra Vote Price Increase", 0f, 10f, 0.5f, MiraNumberSuffixes.None)]
    public float ExtraVotePriceIncrease { get; set; } = 0f;
}