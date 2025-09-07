using PromotionMod.Common;
using PromotionMod.Stats.WitchHunter;
namespace PromotionMod.Elements.PromotionAbilities.WitchHunter;

public class ActNullZone : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatWitchHunter) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.WitchHunterId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActNullZoneId)) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            type = CostType.None,
            cost = 0
        };
    }

    public override bool Perform()
    {
        CC.AddCondition<ConNullZone>();
        CC.AddCooldown(Constants.ActNullZoneId, 10);
        return true;
    }
}