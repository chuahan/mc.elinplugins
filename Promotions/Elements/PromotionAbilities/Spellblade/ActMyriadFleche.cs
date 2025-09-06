using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Spellblade;
using UnityEngine;

namespace PromotionMod.Elements.PromotionAbilities.Spellblade;

public class ActMyriadFleche : ActMelee
{
    public static readonly List<int> PossibleIntonations = new List<int> {
        Constants.EleFire,
        Constants.EleLightning,
        Constants.EleCold,
        Constants.ElePoison,
    };
    
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSpellblade) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SpellbladeId.lang()));
            return false;
        }
        
        bool flag = Act.CC.IsPC && !(Act.CC.ai is GoalAutoCombat);
        if (flag)
        {
            Act.TC = EClass.scene.mouseTarget.card;
        }
        if (Act.TC == null)
        {
            return false;
        }
        if (Act.TC.isThing && !Act.TC.trait.CanBeAttacked)
        {
            return false;
        }
        Act.TP.Set(flag ? EClass.scene.mouseTarget.pos : Act.TC.pos);
        if (Act.CC.isRestrained)
        {
            return false;
        }
        if (Act.CC.host != null || Act.CC.Dist(Act.TP) <= 2)
        {
            return false;
        }
        if (Los.GetRushPoint(Act.CC.pos, Act.TP) == null)
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
        bool flag = Act.CC.IsPC && !(Act.CC.ai is GoalAutoCombat);
        if (flag)
        {
            Act.TC = EClass.scene.mouseTarget.card;
        }
        if (Act.TC == null)
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
            activeIntonation.power = this.GetPower(CC);
            CC.AddCondition(activeIntonation);
        }
        
        // Rush and strike the enemy. 
        Act.TP.Set(flag ? EClass.scene.mouseTarget.pos : Act.TC.pos);
        int num = Act.CC.Dist(Act.TP);
        Point rushPoint = Los.GetRushPoint(Act.CC.pos, Act.TP);
        Act.CC.pos.PlayEffect("vanish");
        Act.CC.MoveImmediate(rushPoint, focus: true, cancelAI: false);
        Act.CC.Say("rush", Act.CC, Act.TC);
        Act.CC.PlaySound("rush");
        Act.CC.pos.PlayEffect("vanish");
        Attack(1f + 0.1f * (float)num);
        
        // Proc a short-ranged breath attack on the target at point-blank to hit surrounding enemies.
        TweenUtil.Delay((float)0.2F, delegate
        {
            CC.PlaySound("spell_breathe");
            if (CC.IsInMutterDistance() && !EClass.core.config.graphic.disableShake)
            {
                Shaker.ShakeCam("breathe");
            }
        });
        List<Point> coneRange = EClass._map.ListPointsInArc(CC.pos, TP, 4, 35f);
        ElementRef elementRef = setting.elements[activeIntonation.source.alias];
        Effect spellEffect = Effect.Get("trail1");
        spellEffect.SetParticleColor(elementRef.colorTrail, changeMaterial: true, "_TintColor").Play(CC.pos);
        spellEffect.sr.color = elementRef.colorSprite;
        TrailRenderer componentInChildren = spellEffect.GetComponentInChildren<TrailRenderer>();
        Color startColor = componentInChildren.endColor = elementRef.colorSprite;
        componentInChildren.startColor = startColor;

        int power = this.GetPower(CC);
        int damage = HelperFunctions.SafeDice("spellblade_myriad_fleche", power);
        foreach (Point p in coneRange)
        {
            spellEffect.Play(CC.pos, 0f, p);
            foreach (Chara chara in p.Charas.Where(chara => chara.IsHostile(CC)))
            {
                // All enemies in the cone will take damage and be inflicted with elemental break of that element.
                HelperFunctions.ProcSpellDamage(power, damage, CC, chara, ele:intonationElement);
                ApplyElementalBreak(intonationElement, chara, power);
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