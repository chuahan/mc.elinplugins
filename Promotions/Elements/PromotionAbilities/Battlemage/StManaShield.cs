using PromotionMod.Common;
using PromotionMod.Stats.Battlemage;
namespace PromotionMod.Elements.PromotionAbilities.Battlemage;

public class StManaShield : PromotionAbility
{
    public override int PromotionId => Constants.FeatBattlemage;
    public override string PromotionString => Constants.BattlemageId;
    public override int AbilityId => Constants.StManaShieldId;

    public override bool Perform()
    {
        if (CC.HasCondition<StanceManaShield>())
        {
            CC.RemoveCondition<StanceManaShield>();
            return true;
        }

        int power = (int)(CC.mana.max * 0.25F);
        CC.AddCondition<StanceManaShield>(power);
        return true;
    }
}