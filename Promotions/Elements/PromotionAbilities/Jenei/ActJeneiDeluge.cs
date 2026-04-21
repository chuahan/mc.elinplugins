using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiDeluge : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatJenei;
    public override string PromotionString => Constants.JeneiId;
    public override int AbilityId => Constants.ActJeneiDelugeId;

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
            origin = CC
        });
        _map.ModLiquid(TC.pos.x, TC.pos.z, 10);
        return true;
    }
}