namespace PromotionMod.Stats;

public class ConCritBoost : ConBuffStats
{
    public override ConditionType Type => ConditionType.Buff;
    public override bool TimeBased => true;
}