using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Spellblade;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Spellblade;

public class ActMyriadFleche : ActMelee
{
    public static readonly List<int> PossibleIntonations = new List<int>
    {
        Constants.EleFire,
        Constants.EleLightning,
        Constants.EleCold,
        Constants.ElePoison
    };

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSpellblade) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SpellbladeId.lang()));
            return false;
        }

        bool flag = CC.IsPC && !(CC.ai is GoalAutoCombat);
        if (flag)
        {
            TC = scene.mouseTarget.card;
        }
        if (TC == null)
        {
            return false;
        }
        if (TC.isThing && !TC.trait.CanBeAttacked)
        {
            return false;
        }
        TP.Set(flag ? scene.mouseTarget.pos : TC.pos);
        if (CC.isRestrained)
        {
            return false;
        }
        if (CC.host != null || CC.Dist(TP) <= 2)
        {
            return false;
        }
        if (Los.GetRushPoint(CC.pos, TP) == null)
        {
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

    public override bool Perform()
    {
        bool flag = CC.IsPC && !(CC.ai is GoalAutoCombat);
        if (flag)
        {
            TC = scene.mouseTarget.card;
        }
        if (TC == null)
        {
            return false;
        }

        // Choose one of the random intonations.
        ConWeapon activeIntonation = CC.GetCondition<ConWeapon>();
        int intonationElement = activeIntonation.sourceElement.id;
        if (activeIntonation == null)
        {
            activeIntonation = new ConWeapon();
            intonationElement = PossibleIntonations.RandomItem();
            activeIntonation.SetElement(intonationElement);
            activeIntonation.power = GetPower(CC);
            CC.AddCondition(activeIntonation);
        }

        // Rush and strike the enemy. 
        TP.Set(flag ? scene.mouseTarget.pos : TC.pos);
        int num = CC.Dist(TP);
        Point rushPoint = Los.GetRushPoint(CC.pos, TP);
        CC.pos.PlayEffect("vanish");
        CC.MoveImmediate(rushPoint, true, false);
        CC.Say("rush", CC, TC);
        CC.PlaySound("rush");
        CC.pos.PlayEffect("vanish");
        Attack(1f + 0.1f * num);

        // Proc a short-ranged breath attack on the target at point-blank to hit surrounding enemies.
        TweenUtil.Delay(0.2F, delegate
        {
            CC.PlaySound("spell_breathe");
            if (CC.IsInMutterDistance() && !core.config.graphic.disableShake)
            {
                Shaker.ShakeCam("breathe");
            }
        });
        List<Point> coneRange = _map.ListPointsInArc(CC.pos, TP, 4, 35f);
        ElementRef elementRef = setting.elements[activeIntonation.source.alias];
        Effect spellEffect = Effect.Get("trail1");
        spellEffect.SetParticleColor(elementRef.colorTrail, true, "_TintColor").Play(CC.pos);
        spellEffect.sr.color = elementRef.colorSprite;
        TrailRenderer componentInChildren = spellEffect.GetComponentInChildren<TrailRenderer>();
        Color startColor = componentInChildren.endColor = elementRef.colorSprite;
        componentInChildren.startColor = startColor;

        int power = GetPower(CC);
        int damage = HelperFunctions.SafeDice("spellblade_myriad_fleche", power);
        foreach (Point p in coneRange)
        {
            spellEffect.Play(CC.pos, 0f, p);
            foreach (Chara chara in p.Charas.Where(chara => chara.IsHostile(CC)))
            {
                // All enemies in the cone will take damage and be inflicted with elemental break of that element.
                HelperFunctions.ProcSpellDamage(power, damage, CC, chara, ele: intonationElement);
                ActMyriadFleche.ApplyElementalBreak(intonationElement, chara, power);
            }
        }
        return true;
    }

    public static void ApplyElementalBreak(int eleId, Chara target, int power)
    {
        switch (eleId)
        {
            case Constants.EleCold:
                target.AddCondition<ConColdBreak>(power);
                return;
            case Constants.EleLightning:
                target.AddCondition<ConLightningBreak>(power);
                return;
            case Constants.EleDarkness:
                target.AddCondition<ConDarknessBreak>(power);
                return;
            case Constants.EleMind:
                target.AddCondition<ConMindBreak>(power);
                return;
            case Constants.ElePoison:
                target.AddCondition<ConPoisonBreak>(power);
                return;
            case Constants.EleNether:
                target.AddCondition<ConNetherBreak>(power);
                return;
            case Constants.EleSound:
                target.AddCondition<ConSoundBreak>(power);
                return;
            case Constants.EleNerve:
                target.AddCondition<ConNerveBreak>(power);
                return;
            case Constants.EleHoly:
                target.AddCondition<ConHolyBreak>(power);
                return;
            case Constants.EleChaos:
                target.AddCondition<ConChaosBreak>(power);
                return;
            case Constants.EleMagic:
                target.AddCondition<ConMagicBreak>(power);
                return;
            case Constants.EleEther:
                target.AddCondition<ConEtherBreak>(power);
                return;
            case Constants.EleAcid:
                target.AddCondition<ConAcidBreak>(power);
                return;
            case Constants.EleCut:
                target.AddCondition<ConCutBreak>(power);
                return;
            case Constants.EleImpact:
                target.AddCondition<ConImpactBreak>(power);
                return;
            default: // And Fire
                target.AddCondition<ConFireBreak>(power);
                return;
        }
    }
}