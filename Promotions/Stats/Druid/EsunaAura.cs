namespace PromotionMod.Stats.Druid;

public class EsunaAura : ConAura
{
    public override bool CanManualRemove => false;

    public override void ApplyInternal(Chara target)
    {
        foreach (Condition condition in target.conditions.Copy())
        {
            if (condition.Type == ConditionType.Debuff &&
                !condition.IsKilled &&
                EClass.rnd(power * 2) > EClass.rnd(condition.power) &&
                condition is not ConWrath && // Don't purge Wrath of God.
                condition is not ConDeathSentense) // Don't purge Death Sentence.
            {
                CC.Say("removeHex", target, condition.Name.ToLower());
                condition.Kill();
                break;
            }
        }
    }
}