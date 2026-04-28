using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities;

public class ActBlessingOfTheDead : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatNecromancer;
    public override string PromotionString => Constants.NecromancerId;
    public override int AbilityId => Constants.ActBlessingOfTheDeadId;

    public override bool CanPerformExtra(bool verbose)
    {
        // Must be a minion of the caster and must be undead.
        if (TC is not { isChara: true } || !TC.IsMinion || TC.Chara.master != CC || !TC.Chara.IsUndead && !TC.IsAliveInCurrentZone)
        {
            if (CC.IsPC && verbose) Msg.Say("necromancer_target_undead_minions".langGame());
            return false;
        }

        return true;
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
        int healthValue = (int)(TC.Chara.hp * (healthPercent / 100F));
        //Msg.Nerun($"Blessing of the Dead Health Percent: {healthPercent}");
        //Msg.Nerun($"Blessing of the Dead Healing: {healthValue}");
        List<Chara> targetsHit = new List<Chara>();
        ElementRef colorRef = setting.elements["eleAcid"];
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

            // Get distance from the origin. Use that to add delay to the explosion.
            Effect spellEffect = Effect.Get("Element/ball_Fire");
            spellEffect.SetParticleColor(colorRef.colorTrail, true, "_TintColor");
            spellEffect.sr.color = colorRef.colorSprite;
            float delay = distance * 0.08F;
            spellEffect.SetStartDelay(delay);
            spellEffect.Play(tile).Flip(tile.x > CC.pos.x);
        }

        TC.Chara.Die();
        return true;
    }
}