using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiDragonPlume : Ability
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
        int damage = HelperFunctions.SafeDice("jenei_dragonplume", power);

        List<Point> coneRange = _map.ListPointsInArc(CC.pos, TP, 4, 35f);
        ElementRef elementRef = setting.elements["eleFire"];
        Effect spellEffect = Effect.Get("trail1");
        spellEffect.SetParticleColor(elementRef.colorTrail, true, "_TintColor").Play(CC.pos);
        spellEffect.sr.color = elementRef.colorSprite;
        TrailRenderer componentInChildren = spellEffect.GetComponentInChildren<TrailRenderer>();
        Color startColor = componentInChildren.endColor = elementRef.colorSprite;
        componentInChildren.startColor = startColor;

        TweenUtil.Tween(0.8F, null, delegate
        {
            CC.PlaySound("spell_breathe");
        });
        if (CC.IsInMutterDistance() && !core.config.graphic.disableShake)
        {
            Shaker.ShakeCam("breathe");
        }

        foreach (Point p in coneRange)
        {
            TweenUtil.Tween(0.06f * CC.pos.Distance(p), null, delegate
            {
                spellEffect.Play(CC.pos, 0f, p);
            });
            foreach (Chara chara in p.Charas.Where(chara => chara.IsHostile(CC)))
            {
                HelperFunctions.ProcSpellDamage(power, damage, CC, TC.Chara, ele: Constants.EleFire, eleP: 75);
            }
        }
        return true;
    }
}