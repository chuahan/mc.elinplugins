using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiDragonFume : Ability
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

    public override bool Perform()
    {
        int power = GetPower(CC);
        List<Point> coneRange = _map.ListPointsInArc(CC.pos, TP, 4, 35f);
        EClass.Wait(0.8f, CC);
        CC.PlaySound("spell_breathe");
        if (CC.IsInMutterDistance() && !EClass.core.config.graphic.disableShake)
        {
            Shaker.ShakeCam("breathe");
        }
        
        ActEffect.DamageEle(CC, EffectId.Breathe, power, Element.Create(Constants.EleFire, power / 10), coneRange, new ActRef()
        {
            act = this,
        });
        return true;
    }
}