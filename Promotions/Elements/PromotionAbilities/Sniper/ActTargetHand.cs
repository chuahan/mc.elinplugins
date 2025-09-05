using PromotionMod.Common;
using PromotionMod.Stats;

namespace PromotionMod.Elements.PromotionAbilities.Sniper;

public class ActTargetHand : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSniper) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SniperId.lang()));
            return false;
        }
        
        return base.CanPerform() && ACT.Ranged.CanPerform();
    }
    
    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        // Snapshot the HP Before and After. This is so I can avoid using a transpiler.
        int currentHP = TC.hp;
        // Perform a Ranged attack at the target.
        ACT.Ranged.Perform(CC, TC);

        // If the HP changed after the attack, we'll consider it a hit.
        if (TC.hp < currentHP)
        {
            TC.Chara.AddCondition<ConDisable>(this.GetPower(CC));
        }
        return true;
    }
}