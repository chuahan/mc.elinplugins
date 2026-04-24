using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
namespace Evolution;

[HarmonyPatch(typeof(Card))]
public class CardPatches
{
    [HarmonyPatch(nameof(Card.MakeEgg))]
    [HarmonyPrefix]
    internal static bool EvolutionMod_PreventMakeEgg_Patch(Card __instance)
    {
        // The Unique Summons and Artificer Golems Should not Drop eggs.
        if (__instance.isChara && __instance.Chara.source.ContainsTag("noEgg"))
        {
            Msg.Say("noEgg_Tag".langGame());
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(Card.Say), typeof(string), typeof(Card), typeof(string), typeof(string))]
    [HarmonyPrefix]
    internal static bool EvolutionMod_Say_SkipNull(Card __instance, string lang, Card c1, string ref1 = null, string ref2 = null)
    {
        if (lang.Equals("item_drop", StringComparison.CurrentCultureIgnoreCase) && c1 is null)
        {
            return false;
        }

        return true;
    }
}