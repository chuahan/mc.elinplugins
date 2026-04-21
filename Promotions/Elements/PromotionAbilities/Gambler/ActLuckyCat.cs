using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Gambler;

/// <summary>
/// </summary>
public class ActLuckyCat : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatGambler;
    public override string PromotionString => Constants.GamblerId;
    public override int AbilityId => Constants.ActLuckyCatId;
}