using System.Collections.Generic;
using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConElementalist : ClassCondition
{
    [JsonProperty(PropertyName = "E")] public Dictionary<int, int> ElementalStockpile = new Dictionary<int, int>
    {
        {
            Constants.EleFire, 0
        },
        {
            Constants.EleCold, 0
        },
        {
            Constants.EleLightning, 0
        },
        {
            Constants.EleDarkness, 0
        },
        {
            Constants.EleMind, 0
        },
        {
            Constants.ElePoison, 0
        },
        {
            Constants.EleNether, 0
        },
        {
            Constants.EleSound, 0
        },
        {
            Constants.EleNerve, 0
        },
        {
            Constants.EleHoly, 0
        },
        {
            Constants.EleChaos, 0
        },
        {
            Constants.EleMagic, 0
        },
        {
            Constants.EleEther, 0
        },
        {
            Constants.EleAcid, 0
        },
        {
            Constants.EleCut, 0
        },
        {
            Constants.EleImpact, 0
        }
    };

    public int GetElementalStrength()
    {
        int totalPower = 0;
        foreach (int elementId in ElementalStockpile.Keys)
        {
            totalPower += ElementalStockpile[elementId];
        }
        return totalPower;
    }

    public int GetElementalCombination()
    {
        int totalTypes = 0;
        foreach (int elementId in ElementalStockpile.Keys)
        {
            if (ElementalStockpile[elementId] > 0)
            {
                totalTypes++;
            }
        }
        return totalTypes;
    }

    // Can accumulate up to 5 of each Element.
    public void AddElementalOrb(int eleId)
    {
        if (ElementalStockpile[eleId] < 5) ElementalStockpile[eleId]++;
    }

    // Upon casting Flare or Elemental Fury, all orbs are consumed.
    public void ConsumeElementalOrbs()
    {
        foreach (int elementId in ElementalStockpile.Keys)
        {
            ElementalStockpile[elementId] = 0;
        }
    }
}