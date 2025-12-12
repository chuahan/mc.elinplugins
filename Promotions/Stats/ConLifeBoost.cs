namespace PromotionMod.Stats;

public class ConLifeBoost : ConBuffStats
{
    public override ConditionType Type => ConditionType.Buff;
    public override bool TimeBased => true;
}