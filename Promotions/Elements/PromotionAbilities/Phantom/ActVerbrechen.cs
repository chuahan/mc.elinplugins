using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats.Phantom;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Phantom;

/// <summary>
/// Rush at a point, deploying Phantom Bits as you move. Reaching the destination, fire up to 8 shots at nearby targets. 25 Stam.
/// Finisher: Fires a high-powered piercing magic beam at the target in a followup. Deploys 2 Magic Phantom Bits.
/// </summary>
public class ActVerbrechen : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatPhantom) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.PhantomId.lang()));
            return false;
        }
        
        // Owner must have a ranged weapon to fire with.
        if (CC.GetBestRangedWeapon() == null) return false;
        
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
    
    public override Cost GetCost(Chara c)
    {
        return new Cost()
        {
            cost = 25,
            type = CostType.SP,
        };
    }

    public override bool Perform()
    {
        Point rushPoint = Los.GetRushPoint(Act.CC.pos, Act.TP);
        // Create 4 Phantom Bits around original location
        for (int i = 0; i < 3; i++)
        {
            FeatPhantom.SpawnPhantomBit(this.GetPower(CC), Act.CC, Act.TP);
        }
        Act.CC.pos.PlayEffect("vanish");
        Act.CC.MoveImmediate(rushPoint, focus: true, cancelAI: false);
        Act.CC.PlaySound("rush");
        Act.CC.pos.PlayEffect("vanish");

        Thing rangedWeapon = CC.GetBestRangedWeapon();
        TraitToolRange traitToolRange = rangedWeapon.trait as TraitToolRange;

        // Fire shots at all nearby enemies from the new position.
        int maxTargets = 8;
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true))
        {
            ConPhantomMark phantomMark = target.GetCondition<ConPhantomMark>();
            int currMarks = phantomMark?.Stacks ?? 0;
            
            // Reload the gun completely before each shot.
            if (traitToolRange.NeedAmmo) rangedWeapon.c_ammo = traitToolRange.MaxAmmo;
            new ActRanged().Perform(CC, target);
            
            FeatPhantom.AddPhantomMarks(target, 1);

            // Trigger Finisher if Target has 10 Phantom Stacks
            if (currMarks == 10)
            {
                // Remove Phantom Mark from the target.
                phantomMark?.Kill();
                
                // Fire a piercing Magic Bolt at the target.
                int damage = HelperFunctions.SafeDice("phantom_verbrechen", this.GetPower(CC));
                List<Point> phantomBeam = EClass._map.ListPointsInLine(CC.pos, target.pos, 10);
                List<Chara> finisherTargets = new List<Chara>();
                
                // Render Beam
                Point from = CC.pos;
                ElementRef elementRef = setting.elements["eleMagic"];
                Effect spellEffect = Effect.Get("trail1");
                spellEffect.SetParticleColor(elementRef.colorTrail, changeMaterial: true, "_TintColor").Play(from);
                spellEffect.sr.color = elementRef.colorSprite;
                TrailRenderer componentInChildren = spellEffect.GetComponentInChildren<TrailRenderer>();
                Color startColor = componentInChildren.endColor = elementRef.colorSprite;
                componentInChildren.startColor = startColor;
                spellEffect.Play(CC.pos, 0f, target.pos);
                
                foreach (Chara beamTarg in from pos in phantomBeam from beamTarg in pos.Charas where !finisherTargets.Contains(beamTarg) select beamTarg)
                {
                    finisherTargets.Add(beamTarg);
                    HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, beamTarg, AttackSource.MagicSword, element: Constants.EleMagic);
                }
                
                // Spawn 2 Phantom Bits
                FeatPhantom.SpawnPhantomBit(this.GetPower(CC), CC, CC.pos);
                FeatPhantom.SpawnPhantomBit(this.GetPower(CC), CC, CC.pos);
            }

            maxTargets--;
            if (maxTargets == 0) break;
        }

        return true;
    }
}