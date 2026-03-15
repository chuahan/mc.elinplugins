using PromotionMod.Common;
namespace PromotionMod.Stats.DreadKnight;

public class ConDarkTraces : ClassCondition
{
    public override int PromotionClass => Constants.FeatDreadKnight;
    
    public override int MaxStacks => 10;
    public override int DecayDelayMax => 3;
    public override string TextDuration => GetStacks().ToString();
    public override bool CanExpire => true;
}