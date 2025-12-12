using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.HolyKnight;

public class StVanguardStance : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHolyKnight) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HolyKnightId.lang()));
            return false;
        }
        return base.CanPerform();
    }
}