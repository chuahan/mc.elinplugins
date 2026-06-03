using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConHeadhunter : ClassCondition
{
    public override int PromotionClass => Constants.FeatHeadhunter;
    public override int MaxStacks => 10;
    public override bool TimeBased => true;
    public override int DecayDelayMax => 5;
    public override string TextDuration => GetStacks().ToString();
}