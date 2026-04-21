using PromotionMod.Common;
using PromotionMod.Stats.Necromancer;
namespace PromotionMod.Elements.PromotionAbilities;

public class ActBeckonOfTheDead : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatNecromancer;
    public override string PromotionString => Constants.NecromancerId;
    public override int AbilityId => Constants.ActBeckonOfTheDeadId;

    public override bool CanPerformExtra()
    {
        if (!TC.isChara || !TC.Chara.IsHostile(CC)) return false;
        return true;
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