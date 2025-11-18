using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats.Phantom;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Phantom;

/// <summary>
///     Rush at a point, deploying Phantom Bits as you move. Reaching the destination, fire up to 8 shots at nearby
///     targets. 25 Stam.
///     Finisher: Fires a high-powered piercing magic beam at the target in a followup. Deploys 2 Magic Phantom Bits.
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

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 12,
            type = CostType.SP
        };
    }

    public override bool Perform()
    {
        Point rushPoint = Los.GetRushPoint(CC.pos, TP);
        // Create 4 Phantom Bits around original location
        for (int i = 0; i < 3; i++)
        {
            FeatPhantom.SpawnPhantomBit(GetPower(CC), CC, TP);
        }
        CC.pos.PlayEffect("vanish");
        CC.MoveImmediate(rushPoint, true, false);
        CC.PlaySound("rush");
        CC.pos.PlayEffect("vanish");

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
            ACT.Ranged.Perform(CC, target);

            FeatPhantom.AddPhantomMarks(target, 1);

            // Trigger Finisher if Target has 10 Phantom Stacks
            if (currMarks == 10)
            {
                int power = GetPower(CC);

                // Remove Phantom Mark from the target.
                phantomMark?.Kill();

                // Fire a piercing Magic Bolt at the target.
                List<Point> phantomBeam = _map.ListPointsInLine(CC.pos, target.pos, 10);

                // Render Beam
                Point from = CC.pos;
                ElementRef elementRef = setting.elements["eleMagic"];
                Effect spellEffect = Effect.Get("trail1");
                spellEffect.SetParticleColor(elementRef.colorTrail, true, "_TintColor").Play(from);
                spellEffect.sr.color = elementRef.colorSprite;
                TrailRenderer componentInChildren = spellEffect.GetComponentInChildren<TrailRenderer>();
                Color startColor = componentInChildren.endColor = elementRef.colorSprite;
                componentInChildren.startColor = startColor;
                spellEffect.Play(CC.pos, 0f, target.pos);

                ActEffect.DamageEle(CC, EffectId.Bolt, power, Element.Create(Constants.EleMagic, power / 10), phantomBeam, new ActRef
                {
                    act = this
                });

                // Spawn 2 Phantom Bits
                FeatPhantom.SpawnPhantomBit(GetPower(CC), CC, CC.pos);
                FeatPhantom.SpawnPhantomBit(GetPower(CC), CC, CC.pos);

                FeatPhantom.PhantomFinisherRestoration(CC);
            }

            maxTargets--;
            if (maxTargets == 0) break;
        }

        return true;
    }
}