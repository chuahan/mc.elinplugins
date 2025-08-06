using PromotionMod.Common;
using PromotionMod.Stats.Berserker;
namespace PromotionMod.Elements.PromotionAbilities.Berserker;

/// <summary>
///     Berserker Ability
///     Similar to Berserk
///     Silences yourself
///     COSTS 25% of current HP to activate.
///     Regenerates 10% HP a turn.
///     Every time you are attacked with melee, counterattack immediately with ActMelee.
/// </summary>
public class ActBloodlust : Ability
{
    public float HealthCost = 0.25F;

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 5,
            type = CostType.SP
        };
    }

    public override bool CanPerform()
    {
        if (CC != null)
        {
            int hpCost = (int)(CC.MaxHP * HealthCost);
            if (CC.hp <= hpCost)
            {
                // You would die if you use this now.
                return false;
            }
        }
        if (CC.Evalue(Constants.FeatBerserker) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.BerserkerId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        int hpCost = (int)(CC.MaxHP * 0.25F);
        CC.DamageHP(hpCost);
        CC.AddCondition<ConBloodlust>();
        return true;
    }
}