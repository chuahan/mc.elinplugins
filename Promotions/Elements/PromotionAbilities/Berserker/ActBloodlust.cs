using PromotionMod.Common;
using PromotionMod.Stats.Berserker;
namespace PromotionMod.Elements.PromotionAbilities.Berserker;

public class ActBloodlust : PromotionAbility
{
    public float HealthCost = 0.25F;
    public override int PromotionId => Constants.FeatBerserker;
    public override string PromotionString => Constants.BerserkerId;
    public override int AbilityId => Constants.ActBloodlustId;

    public override bool CanPerformExtra(bool verbose)
    {
        if (CC != null)
        {
            int hpCost = (int)(CC.MaxHP * HealthCost);
            if (CC.hp <= hpCost)
            {
                if (CC.IsPC && verbose) Msg.Say("hpcostability_notenoughhp".langGame());
                return false;
            }
        }
        return true;
    }

    public override bool Perform()
    {
        int hpCost = (int)(CC.MaxHP * 0.25F);
        CC.hp -= hpCost;
        CC.AddCondition<ConBloodlust>();
        return true;
    }
}