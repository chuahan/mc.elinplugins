using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Berserker;

public class ActSunder : PromotionCombatAbility
{
    public float HealthCost = 0.25F;
    public override int PromotionId => Constants.FeatBerserker;
    public override string PromotionString => Constants.BerserkerId;
    public override int AbilityId => Constants.ActSunderId;


    public override bool CanPerformExtra(bool verbose)
    {
        if (CC != null)
        {
            int hpCost = (int)(CC.MaxHP * HealthCost);
            if (CC.hp <= hpCost)
            {
                if (CC.IsPC && verbose) Msg.Say("hpcostability_notenoughhp".langGame());
                return false;
            }
        }

        return true;
    }

    public override bool Perform()
    {
        int heal = (int)(CC.MaxHP * HealthCost);
        int damage = (int)(TC.Chara.hp * (HealthCost / 2));
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
        CC.HealHP(damage, HealSource.Magic);
        return true;
    }
}