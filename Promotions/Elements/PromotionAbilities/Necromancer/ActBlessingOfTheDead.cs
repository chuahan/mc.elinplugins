using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities;

public class ActBlessingOfTheDead : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatNecromancer) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.NecromancerId.lang()));
            return false;
        }
        // Must be a minion of the caster and must be undead.
        if (!TC.isChara || !TC.IsMinion || TC.Chara.master != CC || !TC.Chara.IsUndead || !TC.IsAliveInCurrentZone) return false;
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
        if (EClass.rnd(4) == 0) CC.TalkRaw($"necromancer_blessing_of_the_dead{EClass.rnd(5)}".langGame());
        if (TC.Chara.id == "sister_undead")
        {
            Msg.Say("jureAngy".langList().RandomItem());
            player.ModKarma(-1);
        }
        // Target's Remaining HP is scaled down.
        int healthPercent = (int)HelperFunctions.SigmoidScaling(GetPower(CC), 40, 75);
        int healthValue = TC.Chara.hp * (healthPercent / 100);
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in _map.ListPointsInCircle(TC.pos, 5f, false, false))
        {
            int distance = tile.Distance(CC.pos);
            foreach (Chara target in tile.ListCharas())
            {
                if (!targetsHit.Contains(target))
                {
                    if (!target.IsHostile(CC))
                    {
                        // Heal HP
                        int missingHP = target.MaxHP - target.hp;
                        if (missingHP >= healthValue)
                        {
                            target.HealHP(healthValue, HealSource.Magic);
                            continue;
                        }
                        // Overheal is converted into Protection
                        target.HealHP(missingHP);
                        int protection = healthValue - missingHP;
                        if (target.HasCondition<ConProtection>())
                        {
                            target.GetCondition<ConProtection>().AddProtection(protection);
                        }
                        else
                        {
                            target.AddCondition<ConProtection>(protection);
                        }
                    }
                }
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion,
            float delay = distance * 0.07F;
            TweenUtil.Delay(delay, delegate
            {
                tile.PlayEffect("Element/ball_Holy");
            });
        }

        TC.Chara.Die();
        return true;
    }
}