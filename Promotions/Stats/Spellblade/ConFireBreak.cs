namespace PromotionMod.Stats.Spellblade;

public class ConFireBreak : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}