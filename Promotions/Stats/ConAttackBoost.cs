namespace PromotionMod.Stats;

public class ConAttackBoost : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Buff;
    public override bool TimeBased => true;
}