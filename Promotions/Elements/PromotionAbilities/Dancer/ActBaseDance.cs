using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

public abstract class ActBaseDance : Act
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatDancer) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.DancerId.lang()));
            return false;
        }

        if (CC.isRestrained || CC.IsDisabled) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        // Dances all share a cooldown.
        CC.AddCooldown(Constants.StEnergyDanceId, 5);
        CC.AddCooldown(Constants.StFreedomDanceId, 5);
        CC.AddCooldown(Constants.StHealingDanceId, 5);
        CC.AddCooldown(Constants.StMistDanceId, 5);
        CC.AddCooldown(Constants.StSwiftDanceId, 5);

        return base.Perform();
    }
}