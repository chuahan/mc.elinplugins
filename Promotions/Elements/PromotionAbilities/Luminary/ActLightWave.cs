using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Luminary;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Luminary;

public class ActLightWave : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatLuminary) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.LuminaryId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActLightWaveId)) return false;

        if (CC.IsPC && !(CC.ai is GoalAutoCombat)) TC = scene.mouseTarget.card;
        if (TC != null) // Can rush to a point without a target.
        {
            if (TC.isThing && !TC.trait.CanBeAttacked) return false;
            TP.Set(TC.pos);
        }

        if (CC.isRestrained) return false;
        if (CC.host != null || CC.Dist(TP) <= 2) return false;
        if (Los.GetRushPoint(CC.pos, TP) == null) return false;

        return base.CanPerform();
    }

    public override bool Perform()
    {
        bool flag = CC.IsPC && !(CC.ai is GoalAutoCombat);
        if (flag)
        {
            TC = scene.mouseTarget.card;
        }

        // Can target a character or a point
        Point rushPoint = TP;
        if (TC != null)
        {
            TP.Set(flag ? scene.mouseTarget.pos : TC.pos);
            int num = CC.Dist(TP);
            rushPoint = Los.GetRushPoint(CC.pos, TP);
        }

        // Select all tiles between the start and the target
        List<Point> affectedPoints = pc.currentZone.map.ListPointsInLine(CC.pos, rushPoint, 2);

        // Render a Holy beam in that direction
        Point from = CC.pos;
        ElementRef elementRef = setting.elements["eleHoly"];
        Effect spellEffect = Effect.Get("trail1");
        spellEffect.SetParticleColor(elementRef.colorTrail, true, "_TintColor").Play(from);
        spellEffect.sr.color = elementRef.colorSprite;
        TrailRenderer componentInChildren = spellEffect.GetComponentInChildren<TrailRenderer>();
        Color startColor = componentInChildren.endColor = elementRef.colorSprite;
        componentInChildren.startColor = startColor;
        spellEffect.Play(CC.pos, 0f, rushPoint);

        // Teleport the user.
        CC.pos.PlayEffect("vanish");
        CC.MoveImmediate(rushPoint, true, false);
        CC.Say("rush", CC, TC);
        CC.PlaySound("rush");
        CC.pos.PlayEffect("vanish");

        // Inflict Holy Damage and Summon a Holy Swordbit
        int power = GetPower(CC);
        ActEffect.DamageEle(CC, EffectId.Sword, power, Element.Create(Constants.EleHoly, power / 10), affectedPoints, new ActRef()
        {
            act = this,
        });
        
        List<Chara> impacted = new List<Chara>();
        foreach (Chara target in from affected in affectedPoints from target in affected.ListCharas() where target.IsHostile(CC) && !impacted.Contains(target) select target)
        {
            impacted.Add(target);
            SpawnHolySwordBit(power, CC, target.pos);
        }

        ActEffect.DamageEle(CC, EffectId.Sword, power, Element.Create(Constants.EleHoly, power / 10), affectedPoints, new ActRef()
        {
            act = this,
        });

        ConLuminary? luminary = CC.GetCondition<ConLuminary>() ?? CC.AddCondition<ConLuminary>() as ConLuminary;
        luminary?.AddStacks(impacted.Count);
        CC.AddCooldown(Constants.ActLightWaveId, 5);
        return true;
    }

    public void SpawnHolySwordBit(int power, Chara caster, Point pos)
    {
        int levelOverride = power / 15;
        if (caster.IsPC) levelOverride = Math.Max(player.stats.deepest, levelOverride);
        Chara summonedBit = CharaGen.Create(Constants.SwordBitCharaId);
        summonedBit.SetMainElement("eleHoly", elemental: true);
        summonedBit.SetSummon(20 + power / 20 + EClass.rnd(10));
        summonedBit.SetLv(levelOverride);
        summonedBit.interest = 0;
        _zone.AddCard(summonedBit, pos.GetNearestPoint(false, false));
        summonedBit.PlayEffect("teleport");
        summonedBit.MakeMinion(caster);
    }
}