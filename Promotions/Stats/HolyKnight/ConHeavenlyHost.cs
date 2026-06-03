using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConHeavenlyHost : ClassCondition
{
    public override int PromotionClass => Constants.FeatHolyKnight;
    public override int MaxStacks => 10;
    public override int DecayDelayMax => 5;
}