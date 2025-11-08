using PromotionMod.Common;
using PromotionMod.Stats.Battlemage;

namespace PromotionMod.Elements.PromotionAbilities.Battlemage;

public class StManaFocus : Ability
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
}