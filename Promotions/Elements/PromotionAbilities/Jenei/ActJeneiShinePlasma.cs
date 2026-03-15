using System.Collections.Generic;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiShinePlasma : Ability
{
    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatJenei))
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