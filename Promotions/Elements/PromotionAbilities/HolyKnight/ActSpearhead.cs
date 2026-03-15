using System;
using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.HolyKnight;
namespace PromotionMod.Elements.PromotionAbilities.HolyKnight;

public class ActSpearhead : ActRush
{
    public override bool CanPerform()
    {
        if (CC == null) return false;
        if (!CC.MatchesPromotion(Constants.FeatHolyKnight))
        {
            Msg.Say("classlocked_ability".lang(Constants.HolyKnightId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActSpearheadId)) return false;

        return base.CanPerform();
    }

    public override bool Perform()
    {
        bool flag = CC.IsPC && !(CC.ai is GoalAutoCombat);
        if (flag) TC = scene.mouseTarget.card;
        if (TC == null) return false;
        TP.Set(flag ? scene.mouseTarget.pos : TC.pos);
        Point rushPoint = Los.GetRushPoint(CC.pos, TP);
        int distance = CC.Dist(TP);

        // Render Holy ball on every tile.
        Effect spellEffect = Effect.Get("Element/ball_Holy");

        // Teleport the user.
        CC.pos.PlayEffect("vanish");
        CC.MoveImmediate(rushPoint, true, false);
        CC.Say("rush", CC, TC);
        CC.PlaySound("rush");
        CC.pos.PlayEffect("vanish");

        // Select all tiles between the start and the target
        List<Point> affectedPoints = pc.currentZone.map.ListPointsInLine(CC.pos, rushPoint, 2);

        // Inflict Holy Damage and Summon a Holy Swordbit
        int power = (int)(GetPower(CC) * (float)(100 + EClass.curve(CC.Evalue(382), 50, 25, 65)) / 100f);
        ActEffect.DamageEle(CC, EffectId.Sword, power, Element.Create(Constants.EleHoly, power / 10), affectedPoints, new ActRef
        {
            act = this
        });

        List<Chara> impacted = new List<Chara>();
        foreach (Point affected in affectedPoints)
        {
            int fxDistance = affected.Distance(CC.pos);
            float delay = fxDistance * 0.7F;
            TweenUtil.Delay(delay, delegate
            {
                spellEffect.Play(affected, 0f, affected);
            });
            foreach (Chara target in affected.Charas)
            {
                if (target.IsHostile(CC) && !impacted.Contains(target))
                {
                    impacted.Add(target);
                    target.TryMoveFrom(target.pos); // Knock back targets
                    ActSpearhead.SpawnHolySwordBit(power, CC, target.pos);
                }
            }
            ;
        }

        float distBonus = 1f + 0.1f * distance;
        distBonus *= (100 + EClass.curve(CC.Evalue(382), 50, 25, 65)) / 100f; // Add Momentum Bonus
        Attack(distBonus);
        ActSpearhead.SpawnHolySwordBit(power, CC, TP);

        ConHeavenlyHost? heavenlyHost = CC.GetCondition<ConHeavenlyHost>() ?? CC.AddCondition<ConHeavenlyHost>() as ConHeavenlyHost;
        heavenlyHost?.AddStacks(impacted.Count);
        CC.AddCooldown(Constants.ActSpearheadId, 5);
        return true;
    }

    public static void SpawnHolySwordBit(int power, Chara caster, Point pos)
    {
        int levelOverride = power / 15;
        if (caster.IsPCFaction) levelOverride = Math.Max(player.stats.deepest, levelOverride);
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