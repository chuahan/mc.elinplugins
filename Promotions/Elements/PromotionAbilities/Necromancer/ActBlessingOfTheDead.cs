using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities;

public class ActBlessingOfTheDead : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatNecromancer;
    public override string PromotionString => Constants.NecromancerId;
    public override int AbilityId => Constants.ActBlessingOfTheDeadId;

    public override bool CanPerformExtra()
    {
        // Must be a minion of the caster and must be undead.
        return TC.isChara && TC.IsMinion && TC.Chara.master == CC && TC.Chara.IsUndead && TC.IsAliveInCurrentZone;
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
                        // Half of Overheal is converted into Protection.
                        target.HealHP(missingHP);
                        int protectionAmount = (healthValue - missingHP) / 2;
                        ConProtection? protection = (ConProtection)(target.GetCondition<ConProtection>() ?? target.AddCondition<ConProtection>());
                        protection?.AddProtection(protectionAmount, true);
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