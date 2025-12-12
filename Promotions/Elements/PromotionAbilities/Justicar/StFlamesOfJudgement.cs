using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Justicar;

public class StFlamesOfJudgement : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJusticar) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.JusticarId.lang()));
            return false;
        }

        // Do not allow the user to use it if they are near death.
        if (CC.hp <= CC.MaxHP * 0.05F) return false;
        return base.CanPerform();
    }
}