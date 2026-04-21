using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiShinePlasma : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatJenei;
    public override string PromotionString => Constants.JeneiId;
    public override int AbilityId => Constants.ActJeneiShinePlasmaId;

    public override bool Perform()
    {
        int power = GetPower(CC);
        Effect lightning = Effect.Get("attack_lightning");
        ElementRef colorRef = setting.elements["eleChaos"];
        lightning.SetParticleColor(colorRef.colorTrail, true, "_TintColor");
        lightning.sr.color = colorRef.colorSprite;
        lightning.Play(TP);

        ActEffect.DamageEle(CC, EffectId.None, power, Element.Create(Constants.EleLightning, power / 10), new List<Point>
        {
            TP
        }, new ActRef
        {
            act = this,
            aliasEle = Constants.ElementAliasLookup[Constants.EleLightning],
            origin = CC
        });
        return true;
    }
}