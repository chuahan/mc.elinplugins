using PromotionMod.Common;
using PromotionMod.Stats.Necromancer;

namespace PromotionMod.Elements.PromotionAbilities;

public class ActBeckonOfTheDead : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatNecromancer) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.NecromancerId.lang()));
            return false;
        }
        if (!TC.isChara || !TC.Chara.IsHostile(CC)) return false; 
        return base.CanPerform();
    }
    
    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        ConDeadBeckon deadBeckon = TC.Chara.AddCondition<ConDeadBeckon>(force: true) as ConDeadBeckon;
        deadBeckon.NecromancerUID = CC.uid;
        return true;
    }
}