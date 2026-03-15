using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.DreadKnight;
namespace PromotionMod.Elements.PromotionAbilities.DreadKnight;

public class ActDarkBarrier : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatDreadKnight) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.DreadKnightId.lang()));
            return false;
        }

        if (!CC.HasCondition<ConDarkTraces>())
        {
            if (CC.IsPC) Msg.Say("dreadknight_notraces".lang());
            return false;
        }

        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
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