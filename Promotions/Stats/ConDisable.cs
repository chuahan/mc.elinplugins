namespace PromotionMod.Stats;

public class ConDisable : BadCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}