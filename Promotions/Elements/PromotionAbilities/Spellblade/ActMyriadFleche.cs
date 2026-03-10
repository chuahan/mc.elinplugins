using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Spellblade;

public class ActMyriadFleche : ActRush
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
        if (CC != null && CC.Evalue(Constants.FeatSpellblade) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SpellbladeId.lang()));
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
        // Choose one of the random intonations if none are active.
        ConWeapon activeIntonation = CC.GetCondition<ConWeapon>();
        int intonationElement = PossibleIntonations.RandomItem();
        
        if (activeIntonation == null)
        {
            activeIntonation = new ConWeapon();
            activeIntonation.SetElement(intonationElement);
            activeIntonation.power = GetPower(CC);
            CC.AddCondition(activeIntonation);
        }
        else
        {
            intonationElement = activeIntonation.sourceElement.id;
        }
        
        // Rush and strike the enemy.
        bool flag = Act.CC.IsPC && !(Act.CC.ai is GoalAutoCombat);
        if (flag) Act.TC = EClass.scene.mouseTarget.card;
        if (Act.TC == null) return false;
        Act.TP.Set(flag ? EClass.scene.mouseTarget.pos : Act.TC.pos);
        Point rushPoint = Los.GetRushPoint(Act.CC.pos, Act.TP);
        Act.CC.pos.PlayEffect("vanish");
        Act.CC.MoveImmediate(rushPoint, focus: true, cancelAI: false);
        Act.CC.Say("rush", Act.CC, Act.TC);
        Act.CC.PlaySound("rush");
        Act.CC.pos.PlayEffect("vanish");
        
        int distance = CC.Dist(TP);
        float distBonus = 1f + 0.1f * (float)distance;
        distBonus *= (float)(100 + EClass.curve(Act.CC.Evalue(382), 50, 25, 65)) / 100f; // Add Momentum Bonus
        Attack(distBonus);
        //new ActMelee().Perform(); //Attack(distBonus);
        
        // Proc a short-ranged breath attack on the target at point-blank to hit surrounding enemies.
        TweenUtil.Delay(0.2F, delegate
        {
            CC.PlaySound("spell_breathe");
            if (CC.IsInMutterDistance() && !core.config.graphic.disableShake)
            {
                Shaker.ShakeCam("breathe");
            }
        });
        
        int power = GetPower(CC);
        List<Point> coneRange = _map.ListPointsInArc(CC.pos, TC.pos, 4, 35f);
        ActEffect.DamageEle(CC, EffectId.Breathe, power, Element.Create(intonationElement, power / 10), coneRange, new ActRef
        {
            act = this
        });

        foreach (Chara target in coneRange.SelectMany(p => p.Charas.Where(target => target.IsHostile(CC))))
        {
            // All enemies in the cone will take damage and be inflicted with elemental break of that element.
            HelperFunctions.ApplyElementalBreak(intonationElement, CC, target, power);
        }
        return true;
    }
}