using PromotionMod.Common;
using PromotionMod.Stats.Hermit;
namespace PromotionMod.Elements.PromotionAbilities.Hermit;

public class ActAssassinate : ActMelee
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHermit) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HermitId.lang()));
            return false;
        }
        // Target must be marked for death and must have 10 Stalk.
        ConMarkedForDeath deathMark = TC.Chara.GetCondition<ConMarkedForDeath>();
        if (deathMark == null || deathMark.Stalk < 10) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        ConMarkedForDeath deathMark = TC.Chara.GetCondition<ConMarkedForDeath>();
        if (deathMark == null) return false;
        int stalkAmount = deathMark.Stalk;
        // Adding Deathbringer guarantees the hit accuracy.
        CC.AddCondition<ConDeathbringer>();
        if (stalkAmount >= 50)
        {
            // 50 Stalk does 2x the Damage, Guaranteed Crit
            Attack(2f, true);
        }
        else if (stalkAmount >= 20)
        {
            // 20 Stalk does 1.25x the Damage. 
            Attack(1.25f, true);
        }
        else
        {
            // Normal hit.
            Attack(1F, true);
        }
        CC.RemoveCondition<ConDeathbringer>();
        return true;
    }
}