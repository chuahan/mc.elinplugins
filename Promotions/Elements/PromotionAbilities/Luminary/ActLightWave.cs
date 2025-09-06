using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
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
        
        if (Act.CC.IsPC && !(Act.CC.ai is GoalAutoCombat)) Act.TC = EClass.scene.mouseTarget.card;
        if (Act.TC != null) // Can rush to a point without a target.
        {
            if (Act.TC.isThing && !Act.TC.trait.CanBeAttacked) return false;
            Act.TP.Set(Act.TC.pos);
        }
        
        if (Act.CC.isRestrained) return false;
        if (Act.CC.host != null || Act.CC.Dist(Act.TP) <= 2) return false;
        if (Los.GetRushPoint(Act.CC.pos, Act.TP) == null) return false;
        
        return base.CanPerform();
    }

    public override bool Perform()
    {
        bool flag = Act.CC.IsPC && !(Act.CC.ai is GoalAutoCombat);
        if (flag)
        {
            Act.TC = EClass.scene.mouseTarget.card;
        }
        
        // Can target a character or a point
        Point rushPoint = Act.TP;
        if (Act.TC != null)
        {
            Act.TP.Set(flag ? EClass.scene.mouseTarget.pos : Act.TC.pos);
            int num = Act.CC.Dist(Act.TP);
            rushPoint = Los.GetRushPoint(Act.CC.pos, Act.TP);
        }
        
        // Select all tiles between the start and the target
        List<Point> affectedPoints = EClass.pc.currentZone.map.ListPointsInLine(Act.CC.pos, rushPoint, 2);

        // Render a Holy beam in that direction
        Point from = CC.pos;
        ElementRef elementRef = setting.elements["eleHoly"];
        Effect spellEffect = Effect.Get("trail1");
        spellEffect.SetParticleColor(elementRef.colorTrail, changeMaterial: true, "_TintColor").Play(from);
        spellEffect.sr.color = elementRef.colorSprite;
        TrailRenderer componentInChildren = spellEffect.GetComponentInChildren<TrailRenderer>();
        Color startColor = componentInChildren.endColor = elementRef.colorSprite;
        componentInChildren.startColor = startColor;
        spellEffect.Play(CC.pos, 0f, rushPoint);
        
        // Teleport the user.
        Act.CC.pos.PlayEffect("vanish");
        Act.CC.MoveImmediate(rushPoint, focus: true, cancelAI: false);
        Act.CC.Say("rush", Act.CC, Act.TC);
        Act.CC.PlaySound("rush");
        Act.CC.pos.PlayEffect("vanish");

        // Inflict Holy Damage and Summon a Holy Swordbit
        int power = this.GetPower(CC);
        int damage = HelperFunctions.SafeDice(Constants.LightwaveAlias, power);
        List<Chara> impacted = new List<Chara>();
        foreach (Chara target in from affected in affectedPoints from target in affected.ListCharas() where target.IsHostile(CC) && !impacted.Contains(target) select target)
        {
            SpawnHolySwordBit(power, CC, target.pos);
            HelperFunctions.ProcSpellDamage(power, damage, CC, target, AttackSource.MagicSword, ele:Constants.EleHoly);
            impacted.Add(target);
        }
        
        ConLuminary? luminary = CC.GetCondition<ConLuminary>() ?? CC.AddCondition<ConLuminary>() as ConLuminary;
        luminary?.AddStacks(impacted.Count);
        CC.AddCooldown(Constants.ActLightWaveId, 5);
        return true;
    }

    public void SpawnHolySwordBit(int power, Chara caster, Point pos)
    {
        int levelOverride = power / 15;
        if (caster.IsPC) levelOverride = Math.Max(EClass.player.stats.deepest, levelOverride);
        Chara summonedBit = CharaGen.Create("swordbit");
        summonedBit.SetMainElement("eleHoly", elemental: true);
        summonedBit.SetSummon(20 + power / 20 + EClass.rnd(10));
        summonedBit.SetLv(levelOverride);
        summonedBit.interest = 0;
        EClass._zone.AddCard(summonedBit, pos.GetNearestPoint(allowBlock: false, allowChara: false));
        summonedBit.PlayEffect("teleport");
        summonedBit.MakeMinion(caster);
    }
}