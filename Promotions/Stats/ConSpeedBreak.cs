namespace PromotionMod.Stats;

public class ConSpeedBreak : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}