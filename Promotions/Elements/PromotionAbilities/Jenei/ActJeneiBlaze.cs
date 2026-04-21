using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiBlaze : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatJenei;
    public override string PromotionString => Constants.JeneiId;
    public override int AbilityId => Constants.ActJeneiBlazeId;

    public override bool Perform()
    {
        int power = GetPower(CC);
        ActEffect.DamageEle(CC, EffectId.None, power, Element.Create(Constants.EleFire, power / 10), new List<Point>
        {
            TP
        }, new ActRef
        {
            act = this
        });
        _map.ModFire(TC.pos.x, TC.pos.z, 10);
        return true;
    }
}