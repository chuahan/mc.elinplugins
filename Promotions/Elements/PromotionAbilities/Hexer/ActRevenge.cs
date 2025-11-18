using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Hexer;

/// <summary>
///     Attempts to consume a debuff on the Hexer and does damage equal to it's power against a target.
///     User must have a debuff to remove.
/// </summary>
public class ActRevenge : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHexer) == 0) return false;
        return CC.Chara.conditions.Count(con => con.Type is ConditionType.Debuff) != 0;
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    // Apply Spell Enhance to this ability.
    public override int GetPower(Card c)
    {
        int power = base.GetPower(c);
        return power * Mathf.Max(100 + c.Evalue(411) - c.Evalue(93), 1) / 100;
    }

    public override bool Perform()
    {
        List<Condition> negativeConditions = CC.Chara.conditions.Where(con => con.Type == ConditionType.Debuff).ToList();
        foreach (Condition debuff in negativeConditions)
        {
            if (debuff.Type != ConditionType.Debuff || debuff.IsKilled || EClass.rnd(GetPower(CC) * 2) <= EClass.rnd(debuff.power)) continue;
            CC.Say("removeHex", TC, debuff.Name.ToLower());
            // Damage Target based on the debuff power + this spellpower.
            int combinedPower = debuff.power + GetPower(CC);
            ActEffect.DamageEle(CC, EffectId.Arrow, combinedPower, Element.Create(Constants.EleMind, combinedPower / 10), new List<Point>
            {
                TC.pos
            }, new ActRef
            {
                act = this
            });
            debuff.Kill();
            return true;
        }
        return true;
    }
}