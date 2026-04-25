using PromotionMod.Common;
using PromotionMod.Stats.WitchHunter;
namespace PromotionMod.Elements.PromotionAbilities.WitchHunter;

public class ActNullZone : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatWitchHunter;
    public override string PromotionString => Constants.WitchHunterId;
    public int Cooldown => 10;
    public override int AbilityId => Constants.ActNullZoneId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;

    public override bool Perform()
    {
        if (CC.HasCondition<ConNullZone>())
        {
            CC.RemoveCondition<ConNullZone>();
            CC.AddCooldown(AbilityId, Cooldown);
            return true;
        }
        CC.AddCondition<ConNullZone>();
        return true;
    }
}