using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiMotherGaia : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatJenei;
    public override string PromotionString => Constants.JeneiId;

    public override int AbilityId => Constants.ActJeneiMotherGaiaId;

    public override bool Perform()
    {
        int power = GetPower(CC);

        TC.pos.Animate(AnimeID.Quake, true);
        CC.PlaySound("spell_earthquake");
        Shaker.ShakeCam("ball");
        ActEffect.DamageEle(CC, EffectId.Earthquake, power, Element.Create(Constants.EleImpact, power / 10), new List<Point>
        {
            TC.pos
        }, new ActRef
        {
            act = this,
            aliasEle = Constants.ElementAliasLookup[Constants.EleImpact],
            origin = CC
        });

        TC.pos.ForeachNeighbor(delegate(Point point)
        {
            ActEffect.DamageEle(CC, EffectId.Earthquake, power, Element.Create(Constants.EleImpact, power / 10), new List<Point>
            {
                point
            }, new ActRef
            {
                act = this,
                aliasEle = Constants.ElementAliasLookup[Constants.EleImpact],
                origin = CC
            });
        });

        return true;
    }
}