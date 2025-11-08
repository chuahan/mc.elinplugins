using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiShinePlasma : Ability
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
        TC.pos.PlayEffect("attack_lightning");
        ActEffect.DamageEle(CC, EffectId.None, power, Element.Create(Constants.EleLightning, power / 10), new List<Point>{TP}, new ActRef()
        {
            act = this,
        });
        return true;
    }
}