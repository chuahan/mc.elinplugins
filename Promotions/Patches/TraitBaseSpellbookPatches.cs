using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(TraitBaseSpellbook))]
public class TraitBaseSpellbookPatches
{
    [HarmonyPatch(nameof(TraitBaseSpellbook.OnRead))]
    [HarmonyPrefix]
    internal static bool OnReadPatch(TraitBaseSpellbook __instance, Chara c)
    {
        // Elementalist - If the player is an elementalist reading an elemental spellbook, they will gain charges of spells in two alternate elements of the same spell type.
        if (c.IsPC && c.Evalue(Constants.FeatElementalist) > 0 && (__instance.BookType == TraitBaseSpellbook.Type.Spell || __instance.BookType == TraitBaseSpellbook.Type.RandomSpell))
        {
            // If it's one of the elemental combat spells, add charges of two other elements.
            if (__instance.source.id is > 50100 and < 50999)
            {
                int currentElement = __instance.source.id % 100;
                int spellType = __instance.source.id / 100;
                
                // Pick first random number.
                int r1 = EClass.rnd(15);
                int firstElement = r1 >= currentElement ? r1 + 1 : r1;

                // Pick second random number.
                int r2 = EClass.rnd(14);
                if (r2 >= Math.Min(currentElement, firstElement)) r2++;
                if (r2 >= Math.Max(currentElement, firstElement)) r2++;
                int secondElement = r2;
                
                c.Say("elementalist_arsenalconspectus".langGame());
                c.GainAbility((spellType * 100 + firstElement), 100, __instance.owner.Thing);
                c.GainAbility((spellType * 100 + secondElement), 100, __instance.owner.Thing);
            };
        }

        return true;
    }
}