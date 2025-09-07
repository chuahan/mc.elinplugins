using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Sniper;

public class ActTargetHead : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSniper) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SniperId.lang()));
            return false;
        }

        return base.CanPerform() && ACT.Ranged.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        // Snapshot the HP Before and After. This is so I can avoid using a transpiler.
        int currentHP = TC.hp;
        // Perform a Ranged attack at the target.
        ACT.Ranged.Perform(CC, TC);

        // If the HP changed after the attack, we'll consider it a hit.
        if (TC.hp < currentHP)
        {
            // 25% chance to cull the target at or under 30% HP.
            if (TC.hp <= TC.MaxHP * 0.30F && EClass.rnd(4) == 0)
            {
                TC.Chara.DamageHP(TC.MaxHP, AttackSource.Finish, CC);
            }
            TC.Chara.AddCondition<ConDim>(GetPower(CC));
            TC.Chara.AddCondition<ConSilence>(GetPower(CC));
        }
        return true;
    }
}