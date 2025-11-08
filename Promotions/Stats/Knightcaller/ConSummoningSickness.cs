namespace PromotionMod.Stats.Knightcaller;

public class ConSummoningSickness : BadCondition
{
    public override ConditionType Type => ConditionType.Bad;
    public override bool TimeBased => true;
}