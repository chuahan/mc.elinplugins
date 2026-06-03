namespace PromotionMod.Stats;

public class ConPoisonBreak : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}