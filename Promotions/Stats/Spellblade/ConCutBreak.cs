namespace PromotionMod.Stats.Spellblade;

public class ConCutBreak : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}