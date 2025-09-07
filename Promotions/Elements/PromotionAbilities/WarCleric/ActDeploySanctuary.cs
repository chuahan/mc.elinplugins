using PromotionMod.Common;
using PromotionMod.Stats.WarCleric;
namespace PromotionMod.Elements.PromotionAbilities.WarCleric;

public class ActDeploySanctuary : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatWarCleric) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.WarClericId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActDeploySanctuaryId)) return false;
        return base.CanPerform();
    }

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
        CC.AddCooldown(Constants.ActDeploySanctuaryId, 30);
        return true;
    }
}