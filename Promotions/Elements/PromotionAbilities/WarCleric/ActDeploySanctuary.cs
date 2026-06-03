using PromotionMod.Common;
using PromotionMod.Stats;

namespace PromotionMod.Elements;

public class ActDeploySanctuary : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatWarCleric;
    public override string PromotionString => Constants.WarClericId;
    public override int AbilityId => Constants.ActDeploySanctuaryId;

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        // Divine Descent Removes cost from this ability
        if (CC.HasCondition<ConDivineDescent>()) convertToMp.cost = 0;
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        CC.AddCondition<ConSanctuary>();
        return true;
    }
}