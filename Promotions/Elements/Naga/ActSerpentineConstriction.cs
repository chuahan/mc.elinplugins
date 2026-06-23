using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements;

public class ActSerpentineConstriction : Ability
{
    public override bool IsHostileAct => true;
    
    public override bool CanPerform()
    {
        // This ability must be used on a character target.
        // We should not allow for Ouroboros where the Nagas decide to choke themselves...?
        if (TC is not { isChara: true } || TC == CC)
        {
            return false;
        }
        
        // We cannot have multiple Nagas choking the same individual.
        // I realize now that having recursive choking is technically now a thing, but the thought of that makes me chuckle, so I'm leaving it in.
        // However, we SHOULD allow it so that if the Naga is currently choking a foo, using the ability on them again will end the bondage.
        ConSerpentConstriction existingConstriction = TC.GetCondition<ConSerpentConstriction>();
        return existingConstriction == null || existingConstriction.LinkedUID == CC.uid;
    }
    
    public override bool Perform()
    {
        // If the Naga re-uses the ability on the same target, end the constriction and that's it.
        ConSerpentConstricting existingConstricting = CC.GetCondition<ConSerpentConstricting>();
        if (existingConstricting != null) {
            if (existingConstricting.LinkedUID == TC.uid)
            {
                CC.RemoveCondition<ConSerpentConstricting>();
                CC.Say("serpentine_constriction_release".langGame(CC.NameSimple, TC.NameSimple));
                return true;
            }
            else
            {
                // We want to make sure the Naga isn't trying to choke multiple targets.
                // If the user already has the Constricting Condition, and it isn't the target, remove the condition (which will end the existing link) and add a new one.
                Chara? previousTarget = HelperFunctions.FindLinkedConditionCarrier(CC, existingConstricting.LinkedUID);
                if (previousTarget != null) CC.Say("serpentine_constriction_release".langGame(CC.NameSimple, previousTarget.NameSimple));
                CC.RemoveCondition<ConSerpentConstricting>();
            }
        }
        
        ConSerpentConstriction victimCondition = TC.Chara.AddCondition<ConSerpentConstriction>(this.GetPower(CC), force:true) as ConSerpentConstriction;
        if (victimCondition != null)
        {
            victimCondition.LinkedUID = CC.uid;
            ConSerpentConstricting userCondition = CC.Chara.AddCondition<ConSerpentConstricting>(force: true) as ConSerpentConstricting;
            userCondition.LinkedUID = TC.uid;
            CC.Say("serpentine_constriction_start".langGame(CC.NameSimple, TC.NameSimple));
        }
        
        return true;
    }
}