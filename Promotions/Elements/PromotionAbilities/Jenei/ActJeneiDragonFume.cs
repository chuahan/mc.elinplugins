using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiDragonFume : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatJenei;
    public override string PromotionString => Constants.JeneiId;
    public override int AbilityId => Constants.ActJeneiDragonFumeId;

    public override bool Perform()
    {
        int power = GetPower(CC);
        List<Point> coneRange = _map.ListPointsInArc(CC.pos, TP, 4, 35f);
        EClass.Wait(0.8f, CC);
        CC.PlaySound("spell_breathe");
        if (CC.IsInMutterDistance() && !core.config.graphic.disableShake)
        {
            Shaker.ShakeCam("breathe");
        }

        ActEffect.DamageEle(CC, EffectId.Breathe, power, Element.Create(Constants.EleFire, power / 10), coneRange, new ActRef
        {
            act = this,
            aliasEle = Constants.ElementAliasLookup[Constants.EleFire],
            origin = CC
        });
        return true;
    }
}