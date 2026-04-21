using PromotionMod.Common;
using PromotionMod.Stats.WarCleric;
namespace PromotionMod.Elements.PromotionAbilities.WarCleric;

public class ActDeploySanctuary : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatWarCleric;
    public override string PromotionString => Constants.WarClericId;

    public override int Cooldown => 100;
    public override int AbilityId => Constants.ActDeploySanctuaryId;

    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatWarCleric))
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
        CC.AddCooldown(AbilityId, Cooldown);
        return true;
    }
}