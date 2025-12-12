using System.Collections.Generic;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiDeluge : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJenei) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.JeneiId.lang()));
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

    // Apply Spell Enhance to this ability.
    public override int GetPower(Card c)
    {
        int power = base.GetPower(c);
        return power * Mathf.Max(100 + c.Evalue(411) - c.Evalue(93), 1) / 100;
    }

    public override bool Perform()
    {
        int power = GetPower(CC);
        ActEffect.DamageEle(CC, EffectId.Puddle, power, Element.Create(Constants.EleCold, power / 10), new List<Point>
        {
            TP
        }, new ActRef
        {
            act = this,
            aliasEle = Constants.ElementAliasLookup[Constants.EleCold],
            origin = CC,
        });
        _map.ModLiquid(TC.pos.x, TC.pos.z, 10);
        return true;
    }
}