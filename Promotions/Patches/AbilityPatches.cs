using System;
using System.Collections.Generic;
using System.Linq;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Battlemage;
using PromotionMod.Stats.Jenei;
using UnityEngine;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Ability))]
public class AbilityPatches
{
    [HarmonyPatch(nameof(Ability.GetPower))]
    [HarmonyPostfix]
    internal static void GetPowerPatch(Ability __instance, ref int __result, Card c)
    {
        if (c is { isChara: true } && c.Evalue(Constants.FeatBattlemage) > 0 && __instance is Spell)
        {
            Chara mage = c.Chara;
            StanceManaFocus focusCon = mage.GetCondition<StanceManaFocus>();
            if (focusCon != null && mage.mana.value > 0)
            {
                // Focus Stance increases power based on current mana. Has Antimage/Spell enhance applied too.
                int spellBoost = (int)(mage.mana.value * 0.15F);
                // TODO: Should I be curving this part too?
                spellBoost = EClass.curve(spellBoost, 400, 100);
                spellBoost = spellBoost * Mathf.Max(100 + mage.Evalue(411) - mage.Evalue(93), 1) / 100;
                __result += spellBoost;
            }
        }
    }
}