using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Artificer;

public class ActTitanCharge : Ability
{
    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 0,
            type = CostType.None
        };
    }

    public override void OnMarkMapHighlights()
    {
        if (!scene.mouseTarget.pos.IsValid)
        {
            return;
        }
        Point dest = scene.mouseTarget.pos;
        Los.IsVisible(pc.pos, dest, delegate(Point p, bool blocked)
        {
            if (!p.Equals(pc.pos))
            {
                p.SetHighlight(blocked || p.IsBlocked || !p.Equals(dest) && p.HasChara ? 4 : p.Distance(pc.pos) <= 2 ? 2 : 8);
            }
        });
    }

    public override bool CanPerform()
    {
        // Usable by Titan Golems or their Riders.
        // Usable by Mounted Cavalry Knight Spirits.
        // If not a Titan Golem, it must be a Chara riding a Titan Golem.
        if (CC.id != Constants.KnightLancerCharaId &&
            CC.id != Constants.TitanGolemCharaId &&
            CC.ride is not { id: Constants.TitanGolemCharaId }) return false;
        if (CC.HasCooldown(Constants.ActTrampleId)) return false;

        if (CC.IsPC && !(CC.ai is GoalAutoCombat)) TC = scene.mouseTarget.card;
        if (TC != null) // Can rush to a point without a target.
        {
            if (TC.isThing && !TC.trait.CanBeAttacked) return false;
            TP.Set(TC.pos);
        }

        if (CC.isRestrained) return false;
        if (CC.host != null || CC.Dist(TP) <= 5) return false;
        if (Los.GetRushPoint(CC.pos, TP) == null) return false;

        return base.CanPerform();
    }

    public override bool Perform()
    {
        // Get the PV and Endurance of the Character involved.
        // The damage roll of this ability PV + End as Dice count and Speed Curved as the Sides.
        Chara actor = CC.id == Constants.TitanGolemCharaId || CC.id == Constants.KnightLancerCharaId ? CC : CC.ride;
        int multiplier = actor.PV * actor.END;
        int power = EClass.curve(actor.Speed, 300, 75);
        long damage = Dice.Create(Constants.ActTrampleAlias, power, actor, act).Roll() * multiplier;

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
            rushPoint = Los.GetRushPoint(CC.pos, TP);
        }

        // Teleport the user.
        CC.pos.PlayEffect("vanish");
        CC.MoveImmediate(rushPoint, true, false);
        CC.Say("rush", CC, TC);
        CC.PlaySound("rush");
        CC.pos.PlayEffect("vanish");

        // Select all hostiles between the start and the target
        List<Chara> impacted = new List<Chara>();
        List<Point> affectedPoints = pc.currentZone.map.ListPointsInLine(CC.pos, TP, 2);
        foreach (Chara target in from affected in affectedPoints from target in affected.ListCharas() where target.IsHostile(CC) && !impacted.Contains(target) select target)
        {
            // Give the target a chance to dodge the attack, using the damage as the hit instead.
            int targetEvasion = EClass.curve(target.PER / 3 + target.Evalue(150), 50, 10) + target.DV + 25;
            if (target.isChara && target.Chara.isBlind) targetEvasion /= 2;
            if (target.HasCondition<ConDim>()) targetEvasion /= 2;
            if (EClass.rnd(damage) < EClass.rnd(targetEvasion))
            {
                impacted.Add(target);
                continue;
            }

            // Calc Perfect Dodge.
            if (target.Evalue(57) > EClass.rnd(100))
            {
                impacted.Add(target);
                continue;
            }

            // Inflict the damage against the targets that don't dodge it.
            target.DamageHP(damage, AttackSource.Melee, CC);
            impacted.Add(target);
        }

        CC.AddCooldown(Constants.ActTrampleId, 5);
        return true;
    }
}