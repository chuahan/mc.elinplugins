using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.DreadKnight;
namespace PromotionMod.Elements.PromotionAbilities.DreadKnight;

public class ActDarkBarrier : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatDreadKnight;
    public override string PromotionString => Constants.DreadKnightId;
    public override int AbilityId => Constants.ActDarkBarrierId;

    public override bool CanPerformExtra(bool verbose)
    {
        if (!CC.HasCondition<ConDarkTraces>())
        {
            if (CC.IsPC && verbose) Msg.Say("dreadknight_notraces".langGame());
            return false;
        }

        return true;
    }

    public override bool Perform()
    {
        ConDarkTraces darkTrace = CC.GetCondition<ConDarkTraces>();
        int protectionAmount = (int)(CC.MaxHP * (darkTrace.GetStacks() * .05F));
        ConProtection? protection = (ConProtection)(pc.GetCondition<ConProtection>() ?? pc.AddCondition<ConProtection>());
        protection?.AddProtection(protectionAmount, true); // This Protection scales power off of HP.
        darkTrace.Kill();
        return true;
    }
}