using PromotionMod.Common;
using PromotionMod.Stats.Necromancer;
namespace PromotionMod.Elements.PromotionAbilities.Necromancer;

public class ActBeckonOfTheDead : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatNecromancer;
    public override string PromotionString => Constants.NecromancerId;
    public override int AbilityId => Constants.ActBeckonOfTheDeadId;

    public override bool CanPerformExtra()
    {
        return TC.isChara && TC.Chara.IsHostile(CC);
    }

    public override bool Perform()
    {
        ConDeadBeckon deadBeckon = TC.Chara.AddCondition<ConDeadBeckon>(force: true) as ConDeadBeckon;
        deadBeckon.NecromancerUID = CC.uid;
        return true;
    }
}