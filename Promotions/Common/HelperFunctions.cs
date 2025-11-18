using System;
using System.Collections.Generic;
using PromotionMod.Stats;
using PromotionMod.Stats.Spellblade;
using UnityEngine;
namespace PromotionMod.Common;

public static class HelperFunctions
{
    public static float SigmoidScaling(float power, float minEffect, float maxEffect, float maxPower = Constants.SigmoidScalingMax, float steepness = Constants.SigmoidScalingPowerSlope)
    {
        float midpoint = maxPower / 2f;
        float normalized = 1f / (1f + Mathf.Exp(-steepness * (power - midpoint)));
        return minEffect + (maxEffect - minEffect) * normalized;
    }

    public static int SafeMultiplier(int value, float multiplier)
    {
        try
        {
            return checked((int)(value * multiplier));
        }
        catch (OverflowException)
        {
            return 2000000000; // Clamp to 2b
        }
    }

    public static int SafeDice(string element, int power, bool rollMax = false)
    {
        Dice dice = Dice.Create(element, power);
        try
        {
            if (rollMax) dice.RollMax();
            return dice.Roll();
        }
        catch (OverflowException)
        {
            return 2000000000; // Clamp to 2b
        }
    }

    public static int SafeDiceForCard(string id, int power, bool rollMax = false, Card c = null, Act act = null)
    {
        Dice dice = Dice.Create(id, power, c, act);
        try
        {
            if (rollMax) dice.RollMax();
            return dice.Roll();
        }
        catch (OverflowException)
        {
            return 2000000000; // Clamp to 2b
        }
    }

    public static int SafeAdd(int value, int addend)
    {
        try
        {
            return checked(value + addend);
        }
        catch (OverflowException)
        {
            return 2000000000; // Clamp to 2b
        }
    }

    public static List<Chara> GetCharasWithinRadius(Point origin, float radius, Chara caster, bool friendly, bool losRequired)
    {
        List<Chara> targets = new List<Chara>();
        foreach (Chara target in EClass.pc.currentZone.map.ListCharasInCircle(origin, radius, losRequired))
        {
            switch (friendly)
            {
                case false when target.IsHostile(caster):
                case true when !target.IsHostile(caster):
                    targets.Add(target);
                    break;
            }
        }

        return targets;
    }

    public static List<Chara> GetCharasWithinRadius(Point origin, float radius, bool friendlyToPC, bool losRequired)
    {
        List<Chara> targets = new List<Chara>();
        foreach (Chara target in EClass.pc.currentZone.map.ListCharasInCircle(origin, radius, losRequired))
        {
            switch (friendlyToPC)
            {
                case false when target.IsHostile(EClass.pc):
                case true when !target.IsHostile(EClass.pc):
                    targets.Add(target);
                    break;
            }
        }

        return targets;
    }

    public static (List<Chara>, List<Chara>) GetOrganizedCharasWithinRadius(Point origin, float radius, Chara caster, bool losRequired)
    {
        List<Chara> friendlies = new List<Chara>();
        List<Chara> enemies = new List<Chara>();

        foreach (Chara target in EClass.pc.currentZone.map.ListCharasInCircle(origin, radius, losRequired))
        {
            if (target.IsHostile(caster))
            {
                enemies.Add(target);
            }
            else
            {
                friendlies.Add(target);
            }
        }

        return (friendlies, enemies);
    }

    // Helper function to do direct spell damage while incorporating Shatter, Control Magic, Protection.
    // This is mostly used for attacks which pass in the damage to deal, or have to go through tools such as artificer tools.
    public static void ProcSpellDamage(int power, long damage, Chara cc, Chara tc, AttackSource attackSource = AttackSource.None, int ele = Constants.EleVoid, int eleP = 100)
    {
        long adjustedDamage = damage;
        // Shatter Reproduction
        bool canShatter = ele != 910 && ele != 911;
        if (cc.IsPCFactionOrMinion && (cc.HasElement(1651) || EClass.pc.Evalue(1651) >= 2)) canShatter = false;
        if (canShatter) EClass._map.TryShatter(tc.pos, ele, power);

        // Defense Reproduction - All of these should be redirectable.
        if (tc.isChara && cc.isChara)
        {
            tc.Chara.RequestProtection(cc.Chara, delegate(Chara a)
            {
                tc = a;
            });
        }

        // Try reducing it by Control Magic if it's a friendly.
        if (tc.IsFriendOrAbove(cc.Chara))
        {
            int controlMagic = cc.Evalue(302);
            if (!cc.IsPC && cc.IsPCFactionOrMinion)
            {
                controlMagic += EClass.pc.Evalue(302);
            }
            if (cc.HasElement(1214)) // Magic Precision
            {
                controlMagic *= 2;
            }
            if (controlMagic > 0)
            {
                if (controlMagic * 10 > EClass.rnd(damage + 1))
                {
                    cc.ModExp(302, cc.IsPC ? 10 : 50);
                    return;
                }
                adjustedDamage = EClass.rnd(damage * 100 / (100 + controlMagic * 10 + 1));
                cc.ModExp(302, cc.IsPC ? 20 : 100);

                if (damage == 0L)
                {
                    return;
                }
            }

            if ((cc.HasElement(1214) || !cc.IsPC && (cc.IsPCFaction || cc.IsPCFactionMinion) && EClass.pc.HasElement(1214)) && EClass.rnd(5) != 0)
            {
                return;
            }
        }

        // Actually inflict the damage.
        tc.DamageHP(adjustedDamage, ele, eleP, attackSource, cc);
    }

    public static Condition Create(string alias, int power, int power2, Chara caster, Action<Condition>? onCreate = null)
    {
        SourceStat.Row row = EClass.sources.stats.alias[alias];
        Condition con = ClassCache.Create<Condition>(row.type.IsEmpty(alias), "Elin");
        con.power = power;
        con.id = row.id;
        con._source = row;
        onCreate?.Invoke(con);
        return con;
    }

    public static string GetRandomEnum<T>() where T : Enum
    {
        string[] names = Enum.GetNames(typeof(T));
        return names.RandomItem();
    }

    public static void ApplyElementalBreak(int eleId, Chara? origin, Chara target, int power)
    {
        // Copied this section from Proc so I can account for their resistance.
        int targetWillScore = target.WIL * (target.IsPowerful ? 20 : 5);
        ConHolyVeil holyVeil = target.GetCondition<ConHolyVeil>();
        if (holyVeil != null)
        {
            targetWillScore += holyVeil.power * 5;
        }

        if (EClass.rnd(power) < targetWillScore / 10 || EClass.rnd(10) == 0)
        {
            target.Say("debuff_resist", target);
            origin?.DoHostileAction(target);
            return;
        }

        switch (eleId)
        {
            case Constants.EleCold:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConColdBreak), power, 10));
                return;
            case Constants.EleLightning:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConLightningBreak), power, 10));
                return;
            case Constants.EleDarkness:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConDarknessBreak), power, 10));
                return;
            case Constants.EleMind:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConMindBreak), power, 10));
                return;
            case Constants.ElePoison:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConPoisonBreak), power, 10));
                return;
            case Constants.EleNether:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConNetherBreak), power, 10));
                return;
            case Constants.EleSound:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConSoundBreak), power, 10));
                return;
            case Constants.EleNerve:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConNerveBreak), power, 10));
                return;
            case Constants.EleHoly:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConHolyBreak), power, 10));
                return;
            case Constants.EleChaos:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConMagicBreak), power, 10));
                return;
            case Constants.EleMagic:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConColdBreak), power, 10));
                return;
            case Constants.EleEther:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConEtherBreak), power, 10));
                return;
            case Constants.EleAcid:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConAcidBreak), power, 10));
                return;
            case Constants.EleCut:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConCutBreak), power, 10));
                return;
            case Constants.EleImpact:
                target.AddCondition(SubPoweredCondition.Create(nameof(ConImpactBreak), power, 10));
                return;
            default: // And Fire
                target.AddCondition(SubPoweredCondition.Create(nameof(ConFireBreak), power, 10));
                return;
        }
    }
}