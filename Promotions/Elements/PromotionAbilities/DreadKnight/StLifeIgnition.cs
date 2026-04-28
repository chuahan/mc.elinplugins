using PromotionMod.Common;
using PromotionMod.Stats.DreadKnight;
namespace PromotionMod.Elements.PromotionAbilities.DreadKnight;

public class StLifeIgnition : PromotionAbility
{
    public override int PromotionId => Constants.FeatDreadKnight;
    public override string PromotionString => Constants.DreadKnightId;
    public override int AbilityId => Constants.StLifeIgnitionId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;

    public override bool CanPerformExtra(bool verbose)
    {

        if (CC != null)
        {
            int hpCost = (int)(CC.MaxHP * 0.1F);
            if (CC.hp <= hpCost)
            {
                // You would die if you use this now.
                if (CC.IsPC && verbose) Msg.Say("hpcostability_notenoughhp".langGame());
                return false;
            }
        }

        return true;
    }

    public override bool Perform()
    {
        StanceLifeIgnition existingStance = CC.GetCondition<StanceLifeIgnition>();
        if (existingStance != null)
        {
            existingStance.Kill();
        }
        else
        {
            CC.AddCondition<StanceLifeIgnition>();
        }
        return true;
    }
}