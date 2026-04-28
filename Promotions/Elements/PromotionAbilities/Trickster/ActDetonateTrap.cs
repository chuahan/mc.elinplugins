using PromotionMod.Common;
using PromotionMod.Trait.Trickster;
namespace PromotionMod.Elements.PromotionAbilities.Trickster;

public class ActDetonateTrap : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatTrickster;
    public override string PromotionString => Constants.TricksterId;
    public override int AbilityId => Constants.ActDetonateTrapId;

    public override int PerformDistance => 5;

    public override bool CanPerformExtra(bool verbose)
    {
        if (TP.Installed == null || TP.GetInstalled<TraitTricksterArcaneTrap>() == null)
        {
            if (CC.IsPC && verbose) Msg.Say("trickster_musttargettrap".langGame());
            return false;
        }
        return true;
    }

    // Leave this at 0 so it doesn't level up. The main cost is the trap itself.
    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            type = CostType.None,
            cost = 0
        };
    }

    public override bool Perform()
    {
        TraitTricksterArcaneTrap arcaneTrap = TP.GetInstalled<TraitTricksterArcaneTrap>();
        arcaneTrap.DetonateTrap(true);
        return true;
    }
}