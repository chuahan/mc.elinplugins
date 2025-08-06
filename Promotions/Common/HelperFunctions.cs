using System;
using System.Collections.Generic;
using UnityEngine;
namespace PromotionMod.Common;

public static class HelperFunctions
{

    public static float SigmoidScaling(float power, float maxPower, float minEffect, float maxEffect, float steepness)
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
}