namespace PromotionMod.Stats;

public class ConDarknessBreak : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}