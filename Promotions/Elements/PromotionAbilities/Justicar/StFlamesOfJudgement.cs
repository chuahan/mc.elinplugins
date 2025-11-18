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
        return base.CanPerform();
    }
}