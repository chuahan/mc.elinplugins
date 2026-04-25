using System;
using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Headhunter;
namespace PromotionMod.Elements.PromotionAbilities.Headhunter;

/// <summary>
///     Headhunter Ability
///     Melee attack that will instantly kill any enemies at or below 25% HP.
/// </summary>
public class ActExecute : PromotionCombatAbility
{

    public float CullThreshold = 0.25f;
    public override int PromotionId => Constants.FeatHeadhunter;
    public override string PromotionString => Constants.HeadhunterId;
    public int Cooldown => 10;
    public override int AbilityId => Constants.ActExecuteId;

    public override bool CanPerformExtra()
    {

        if (CC == TC || TC is not { isChara: true } || CC.Dist(TC) > 1)
        {
            return false;
        }
        return true;
    }

    // Cost is reduced by 10% per Headhunter stack are active.
    public override Cost GetCost(Chara c)
    {
        Cost cost = base.GetCost(c);
        if (CC != null && CC.HasCondition<ConHeadhunter>())
        {
            int reduction = (int)(cost.cost * 0.1);
            cost.cost -= CC.GetCondition<ConHeadhunter>().GetStacks() * reduction;
            cost.cost = Math.Min(1, cost.cost);
        }
        return cost;
    }

    public override bool Perform()
    {
        // Perform a Melee Attack.
        new ActMelee().Perform(CC, TC);

        // Cull enemy if possible.
        if (TC.MaxHP * CullThreshold >= TC.hp)
        {
            CC.PlaySound("hit_finish");
            CC.Say("finish");
            CC.Say("finish2", CC, TC);
            TC.DamageHP(TC.MaxHP, AttackSource.Finish, CC);

            // If the cull was successful, do not add a cooldown and refund stamina cost.
            // Apply Momentum to all allies.
            CC.stamina.Mod(GetCost(CC).cost);

            List<Chara> affectedAllies = HelperFunctions.GetCharasWithinRadius(CC.pos, 3, CC, true, true);
            foreach (Chara target in affectedAllies)
            {
                target.AddCondition<ConMomentum>(100, true);
            }
        }
        else
        {
            // Add a cooldown.
            CC.AddCooldown(AbilityId, Cooldown - CC.GetCondition<ConHeadhunter>()?.GetStacks() ?? 0);
        }

        return true;
    }
}