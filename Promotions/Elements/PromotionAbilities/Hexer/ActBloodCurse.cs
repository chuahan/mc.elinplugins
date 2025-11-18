using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Hexer;

// Force applies one of the curses randomly at the cost of 10% life. Will prioritize curses you have not applied of the same tier that you roll.
public class ActBloodCurse : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHexer) == 0) return false;
        if (CC.hp <= CC.MaxHP * 0.1F) return false;
        if (TC == null) return false;
        return true;
    }

    // This ability doesn't cost MP or Stamina, and instead costs 10% HP. 
    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 0,
            type = CostType.None
        };
    }

    // Apply Spell Enhance to this ability.
    public override int GetPower(Card c)
    {
        int power = base.GetPower(c);
        return power * Mathf.Max(100 + c.Evalue(411) - c.Evalue(93), 1) / 100;
    }

    public override bool Perform()
    {
        FeatHexer.ApplyCondition(TC.Chara, CC, GetPower(CC), true);
        int hpCost = (int)(CC.MaxHP * 0.1F);
        CC.DamageHP(hpCost, AttackSource.Burden);
        return true;
    }
}