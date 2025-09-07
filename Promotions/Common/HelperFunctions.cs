using System;
using System.Collections.Generic;
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

    public static int SafeDice(string element, int power)
    {
        Dice dice = Dice.Create(element, power);
        try
        {
            return dice.Roll();
        }
        catch (OverflowException)
        {
            return 2000000000; // Clamp to 2b
        }
    }

    public static int SafeDice(string id, int power, Card c = null, Act act = null)
    {
        Dice dice = Dice.Create(id, power, c, act);
        try
        {
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

    // Helper function to do all the extra effects when calculating spell stuff.
    public static void ProcSpellDamage(int power, int damage, Chara cc, Chara tc, AttackSource attackSource = AttackSource.None, int ele = Constants.EleVoid, int eleP = 100)
    {
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

        // Actually inflict the damage.
        tc.DamageHP(damage, ele, eleP, attackSource, cc);
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
}