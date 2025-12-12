using PromotionMod.Common;
using PromotionMod.Stats.Hermit;
namespace PromotionMod.Elements.PromotionAbilities.Hermit;

public class ActAssassinate : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHermit) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HermitId.lang()));
            return false;
        }
        // Must have a Target. Target must be marked for death with at least 10 value.
        if (TC == null) return false;
        
        ConMarkedForDeath deathMark = TC.Chara.GetCondition<ConMarkedForDeath>();
        if (deathMark == null || deathMark.value < 10) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        ConMarkedForDeath deathMark = TC.Chara.GetCondition<ConMarkedForDeath>();
        if (deathMark == null) return false;
        int stalkAmount = deathMark.value;
        // Adding Deathbringer guarantees the hit accuracy.
        CC.AddCondition<ConDeathbringer>();
        if (stalkAmount >= 30)
        {
            // 30 Stalk does 2x the Damage, Guaranteed Crit
            new ActMeleeVitalAssassination().Perform(CC, TC);
        }
        else if (stalkAmount >= 20)
        {
            // 20 Stalk does 1.25x the Damage. 
            new ActMeleeAssassination().Perform(CC, TC);
        }
        else
        {
            // Normal hit.
            new ActMelee().Perform(CC, TC);
        }
        CC.RemoveCondition<ConDeathbringer>();
        return true;
    }
}