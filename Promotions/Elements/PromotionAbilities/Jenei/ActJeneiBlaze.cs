using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiBlaze : Ability
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
        ActEffect.DamageEle(CC, EffectId.None, power, Element.Create(Constants.EleFire, power / 10), new List<Point>{TP}, new ActRef()
        {
            act = this,
        });
        _map.ModFire(TC.pos.x, TC.pos.z, 10);
        return true;
    }
}