using PromotionMod.Common;
using PromotionMod.Stats.DreadKnight;
namespace PromotionMod.Elements.PromotionAbilities.DreadKnight;

public class ActManaStarter : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatDreadKnight;
    public override string PromotionString => Constants.DreadKnightId;
    public override int AbilityId => Constants.ActManaStarterId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;

    public override bool CanPerformExtra()
    {
        if (GetHPCost(CC) > CC.hp)
        {
            if (CC.IsPC) Msg.Say("dreadknight_notenoughhp".langGame());
            return false;
        }

        return true;
    }

    public int GetHPCost(Chara c)
    {
        ConDarkTraces darkTrace = CC.GetCondition<ConDarkTraces>();
        float hpCost = 0.1F;
        if (darkTrace != null)
        {
            // Dark Traces increases the HP Cost by 10% each stack
            hpCost += darkTrace.GetStacks() * .1F;
        }

        return (int)(c.MaxHP * hpCost);
    }

    public override bool Perform()
    {
        ConDarkTraces darkTrace = CC.GetCondition<ConDarkTraces>();
        float hpCost = 0.1F;
        if (darkTrace != null)
        {
            // Dark Traces increases the HP Cost by 10% each stack
            hpCost += darkTrace.GetStacks() * .1F;
        }
        else
        {
            darkTrace = (ConDarkTraces)CC.AddCondition<ConDarkTraces>();
        }

        // User will restore 2x the cost in Mana.
        int cost = (int)(CC.MaxHP * hpCost);

        CC.hp -= cost;
        CC.mana.Mod(cost * 2);

        darkTrace.AddStacks(1);
        return true;
    }
}