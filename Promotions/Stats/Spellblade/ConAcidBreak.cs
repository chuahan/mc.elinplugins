namespace PromotionMod.Stats.Spellblade;

public class ConAcidBreak : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}