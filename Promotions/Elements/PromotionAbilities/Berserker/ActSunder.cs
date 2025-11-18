using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Berserker;

public class ActSunder : Ability
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
        int heal = (int)(CC.MaxHP * HealthCost);
        int damage = (int)(TC.Chara.MaxHP * HealthCost);
        List<Condition> casterDebuffs = CC.Chara.conditions.Where(x =>
                x.Type is ConditionType.Debuff or ConditionType.Bad && x is not ConDeathSentense && x is not ConWrath && x is not ConAnorexia && x is not ConSuspend).ToList();
        List<Condition> targetDebuffs = TC.Chara.conditions.Where(x =>
                x.Type is ConditionType.Debuff or ConditionType.Bad && x is not ConDeathSentense && x is not ConWrath && x is not ConAnorexia && x is not ConSuspend).ToList();
        for (int i = 0; i < casterDebuffs.Count; i++)
        {
            TC.Chara.AddCondition(casterDebuffs[i].source.alias, casterDebuffs[i].power, true);
            casterDebuffs[i].Kill();
        }
        for (int i = 0; i < targetDebuffs.Count; i++)
        {
            CC.Chara.AddCondition(targetDebuffs[i].source.alias, targetDebuffs[i].power, true);
            targetDebuffs[i].Kill();
        }

        TC.DamageHP(damage, AttackSource.Melee, CC);
        CC.HealHP(heal, HealSource.Magic);
        CC.AddCooldown(Constants.ActSunderId, 10);
        return true;
    }
}