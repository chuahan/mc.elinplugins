using PromotionMod.Common;
using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

public abstract class ActBaseDance<TStance> : Act
        where TStance : StanceDance, new()
{
    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatDancer))
        {
            Msg.Say("classlocked_ability".lang(Constants.DancerId.lang()));
            return false;
        }

        if (CC.isRestrained || CC.IsDisabled) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        bool danceStopped = false;

        // Remove any other dances currently being performed.
        for (int i = CC.conditions.Count - 1; i >= 0; i--)
        {
            // Stop any other StanceDance. There should only be one active.
            if (CC.conditions[i] is StanceDance dance)
            {
                if (CC.conditions[i] is TStance) danceStopped = true;
                CC.conditions[i].Kill();
                break;
            }
        }

        if (danceStopped) return true;
        // Add the Dance Stance
        CC.AddCondition<TStance>();
        // Start the Cooldowns. They all share a cooldown.
        CC.AddCooldown(Constants.StEnergyDanceId, 5);
        CC.AddCooldown(Constants.StFreedomDanceId, 5);
        CC.AddCooldown(Constants.StHealingDanceId, 5);
        CC.AddCooldown(Constants.StMistDanceId, 5);
        CC.AddCooldown(Constants.StSwiftDanceId, 5);
        return true;
    }
}