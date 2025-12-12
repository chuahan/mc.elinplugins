using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Stats;

public class ConElementalist : ClassCondition
{

    // When you haven't gained elemental orbs in the past 5 turns, you will lose one random stockpiled orb a turn.
    private const int DecayDelayMax = 5;

    [JsonProperty(PropertyName = "R")] private int _decayDelay;

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

    public int GetPower(Chara cc)
    {
        int basePower = cc.LV * 6 + 100; // Level * 6 + 100
        basePower += cc.Evalue(76) * 4; // Add MAG stat * 4
        basePower = EClass.curve(basePower, 400, 100); // Curve
        return basePower * Mathf.Max(100 + cc.Evalue(411) - cc.Evalue(93), 1) / 100; // Add Spellpower + AntiMag
    }

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

    /// <summary>
    ///     Can accumulate up to 5 of each Element.
    ///     If gaining the elemental orb for the first time, gain a stack of Spell Tempo (caps at 10.)
    /// </summary>
    public void AddElementalOrb(int eleId, Chara tc)
    {
        if (ElementalStockpile[eleId] == 0)
        {
            ElementalStockpile[eleId]++;
            int tempoStage = 1;
            ConSpellTempo currentTempo = owner.GetCondition<ConSpellTempo>();
            if (currentTempo != null)
            {
                tempoStage += currentTempo.power;
                if (tempoStage > 10) tempoStage = 10;
                currentTempo.Kill();
            }

            owner.AddCondition<ConSpellTempo>(tempoStage);
            HelperFunctions.ApplyElementalBreak(eleId, owner, tc, GetPower(owner));
        }
        if (ElementalStockpile[eleId] < 5) ElementalStockpile[eleId]++;
        _decayDelay = 0;
    }

    // Upon casting Flare or Elemental Fury, all orbs are consumed.
    public void ConsumeElementalOrbs()
    {
        foreach (int elementId in ElementalStockpile.Keys.ToList())
        {
            ElementalStockpile[elementId] = 0;
        }
        _decayDelay = 0;
    }

    public override void Tick()
    {
        if (_decayDelay == DecayDelayMax)
        {
            // Lose a random orb.
            List<int> elementsWithOrbs = ElementalStockpile.Where(s => s.Value > 0).Select(s => s.Key).ToList();
            if (elementsWithOrbs.Count == 0) return;
            ElementalStockpile[elementsWithOrbs.RandomItem()]--;
        }
        else if (_decayDelay < DecayDelayMax)
        {
            _decayDelay++;
        }
    }
    
    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintElementalist".lang());
        foreach (int elementId in ElementalStockpile.Keys)
        {
            if (ElementalStockpile[elementId] > 0) list.Add("elementalistOrbCount".lang(EClass.sources.elements.map[elementId].GetName(), ElementalStockpile[elementId].ToString()));
        }
    }
}