using System;
using System.Collections.Generic;
using System.Linq;
using BardMod.Source;
using UnityEngine;

namespace BardMod.Common.HelperFunctions;

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
        Dice dice = Dice.Create(element, power, null, null);
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
        foreach (Point item in EClass.pc.currentZone.map.ListPointsInCircle(origin, radius, mustBeWalkable: false,
                     los: losRequired))
        {

            List<Chara> pointCharacters = item.detail?.charas;
            if (pointCharacters == null || pointCharacters.Count == 0) continue;

            foreach (Chara listener in pointCharacters)
            {
                switch (friendly)
                {
                    case false when listener.IsHostile(caster):
                    case true when !listener.IsHostile(caster):
                        targets.Add(listener);
                        break;
                }
            }
        }
        

        return targets;
    }
    
    public static int GetCumulativeBardSkillLevel()
    {
        List<int> allBardSkills = new List<int>
        {
            Constants.BardStrengthSongId,
            Constants.BardSpeedSongId,
            Constants.BardHealingSongId,
            Constants.BardMagicSongId,
            Constants.BardGuardSongId,
            Constants.BardDishearteningSongId,
            Constants.BardChaosSongId,
            Constants.BardDisorientationSongId,
            Constants.BardWitheringSongId,
            Constants.BardSleepSongId,
            Constants.BardLuckSongId,
            Constants.BardVigorSongId,
            Constants.BardMirrorSongId,
            Constants.BardShellSongId,
            Constants.BardDisruptionSongId,
            Constants.BardScathingSongId,
            Constants.BardDrowningSongId,
            Constants.BardWitchHuntSongId,
            Constants.BardPuritySongId,
            Constants.BardSlashSongId,
            Constants.BardKnockbackSongId,
            Constants.BardEchoSongId,
            Constants.BardDispelSongId,
            Constants.BardCheerSongId,
            Constants.BardTuningSongId,
            Constants.BardElementalSongId,
            Constants.BardFinaleSongId,
        };

        int cumulativeBardSkill = 0;
        foreach (int skill in allBardSkills)
        {
            cumulativeBardSkill += EClass.player.chara.Evalue(skill);
        }

        return cumulativeBardSkill;
    }

    public static List<Thing> GetAllInstruments()
    {
        // If we have multiple instruments, search for the one with the flag IsSelectedInstrument. Instruments can be anywhere in inventory, including substorage.
        // Search in Inventory
        List<Thing> allInstruments = EClass.pc.things.FindAll(x => x.trait is TraitToolBard);
		
        // Search in Toolbelt.
        Thing toolbelt = EClass.pc.things.Find(x => x.trait is TraitToolBelt);
        allInstruments.AddRange(toolbelt.things.FindAll(x => x.trait is TraitToolBard));
		
        // Storage in Inventory
        List<Thing> inventoryStorage = EClass.pc.things.FindAll(x => x.trait is TraitContainer);
        foreach (Thing inventoryStorageContainer in inventoryStorage)
        {
            allInstruments.AddRange(inventoryStorageContainer.things.FindAll(x => x.trait is TraitToolBard));
        }
		
        // Storage in Toolbelt
        List<Thing> toolBeltStorage = toolbelt.things.FindAll(x => x.trait is TraitContainer);
        foreach (Thing toolBeltContainer in toolBeltStorage)
        {
            allInstruments.AddRange(toolBeltContainer.things.FindAll(x => x.trait is TraitToolBard));
        }

        return allInstruments;
    }
}