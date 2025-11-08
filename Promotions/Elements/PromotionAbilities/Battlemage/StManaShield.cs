using PromotionMod.Common;
using PromotionMod.Stats.Battlemage;

namespace PromotionMod.Elements.PromotionAbilities.Battlemage;

public class StManaShield : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatBattlemage) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.BattlemageId.lang()));
            return false;
        }

        return base.CanPerform();
    }

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