namespace PromotionMod.Stats;

public class ConAttackBreak : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}