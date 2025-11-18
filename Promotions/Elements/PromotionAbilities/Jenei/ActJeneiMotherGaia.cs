using System.Collections.Generic;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiMotherGaia : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJenei) == 0) return false;
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

        List<Point> targets = new List<Point>
        {
            TC.pos
        };

        _map.ForeachNeighbor(TC.pos, delegate(Point neighbor)
        {
            TweenUtil.Tween(0.8F, null, delegate
            {
                targets.Add(neighbor);
                neighbor.Animate(AnimeID.Quake, true);
            });
        });

        TC.pos.Animate(AnimeID.Quake, true);
        CC.PlaySound("spell_earthquake");
        Shaker.ShakeCam("ball");

        ActEffect.DamageEle(CC, EffectId.Earthquake, power, Element.Create(Constants.EleImpact, power / 10), targets, new ActRef
        {
            act = this
        });
        return true;
    }
}