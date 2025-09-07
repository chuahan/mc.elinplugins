namespace PromotionMod.Stats.Spellblade;

public class ConMagicBreak : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}