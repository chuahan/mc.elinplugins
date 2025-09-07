namespace PromotionMod.Stats.Spellblade;

public class ConColdBreak : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}