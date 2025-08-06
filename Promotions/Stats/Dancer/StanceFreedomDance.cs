using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.Abilities.Dancer;

public class StanceFreedomDance : StanceDance
{
    public override void ActInternal(Chara target, int power, bool isPartner)
    {
        // Remove up to 2 buffs a turn for partner, 1 buff a turn for non partners.
        // If Partner,
        int buffsRemoved = isPartner ? 2 : 1;
        foreach (Condition item5 in target.conditions.Copy())
        {
            if (item5.Type == ConditionType.Debuff &&
                !item5.IsKilled &&
                EClass.rnd(power * 2) > EClass.rnd(item5.power) &&
                item5 is not ConWrath && // Don't purge Wrath of God.
                item5 is not ConDeathSentense) // Don't purge Death Sentence.
            {
                CC.Say("removeHex", target, item5.Name.ToLower());
                item5.Kill();
                buffsRemoved--;
                if (buffsRemoved == 0)
                {
                    break;
                }
            }
        }
    }
}