using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Justicar;

// TODO: Need to Sprite in SFX.
public class StFlamesOfJudgement : PromotionAbility
{
    public override int PromotionId => Constants.FeatJusticar;
    public override string PromotionString => Constants.JusticarId;
    public override int AbilityId => Constants.StFlamesOfJudgementId;

    public override bool CanPerformExtra()
    {
        // Do not allow the user to use it if they are near death.
        if (CC.hp <= CC.MaxHP * 0.05F) return false;
        return true;
    }
}