namespace PromotionMod.Stats.Spellblade;

public class ConEtherBreak : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}