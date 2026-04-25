using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.AdvCombatSkills;
using PromotionMod.Stats.DreadKnight;
using PromotionMod.Stats.Hexer;
using PromotionMod.Stats.Sentinel;
using PromotionMod.Trait;
using PromotionMod.Trait.Artificer;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Card))]
public class CardPatches
{
    [HarmonyPatch(nameof(Card.ModExp), typeof(int), typeof(int))]
    [HarmonyPrefix]
    internal static bool AdventurerExpBoost(Card __instance, int ele, ref int a)
    {
        if (__instance.isChara)
        {
            if (__instance.Chara.IsPCFaction &&
                EClass.pc.MatchesPromotion(Constants.FeatAdventurer))
            {
                // Adventurers increase the exp gain by 50%
                a = (int)(a * 1.5F);
            }
        }
        return true;
    }

    [HarmonyPatch(typeof(Card), "get_PV")]
    [HarmonyPostfix]
    internal static void PvPatches(Card __instance, ref int __result)
    {
        // Battlemage - Mana Armor
        if (__instance.isChara)
        {
            Chara chara = __instance.Chara;
            if (chara.MatchesPromotion(Constants.FeatBattlemage))
            {
                int manaValue = chara.mana.value / 5;
                __result += manaValue;
            }

            // Sentinel - Increased PV for Heavy Armor and/or Shield.
            if (chara.MatchesPromotion(Constants.FeatSentinel))
            {
                // Increase PV by 50% if wearing Heavy Armor
                if (chara.GetArmorSkill() == 122)
                {
                    __result = HelperFunctions.SafeMultiplier(__result, 1.5F);
                }

                // Increase PV by 50% if using a Shield
                if (chara.body.GetAttackStyle() == AttackStyle.Shield)
                {
                    __result = HelperFunctions.SafeMultiplier(__result, 1.5F);
                }

                // PV is set to 0 when in Rage Stance.
                if (chara.HasCondition<StanceRage>())
                {
                    __result = 0;
                }
            }
        }
    }

    [HarmonyPatch(nameof(Card.HealHP))]
    [HarmonyPrefix]
    internal static bool OnHealHP(Card __instance, ref int a, HealSource origin)
    {
        // Trickster - Despair Debuff prevents healing.
        if (__instance.isChara)
        {
            Chara chara = __instance.Chara;
            // Build condition dictionaries for fast lookup
            Dictionary<Type, Condition> targetConditions = chara.conditions.GroupBy(c => c.GetType()).ToDictionary(g => g.Key, g => g.First());

            if (targetConditions.ContainsKey(typeof(ConCorruption)))
            {
                chara.Say("trickster_corruption_nullheal".langGame());
                return false;
            }

            // War Cleric - Healing is copied to nearby allies with 50% efficacy.
            // Does not trigger on HealSource.None, which is Regen, or duplicated healing, or mods who don't actually add a HealSource.
            if (__instance.Chara.MatchesPromotion(Constants.FeatWarCleric) && origin != HealSource.None)
            {
                chara.Say("warcleric_healshare".langGame());
                foreach (Chara ally in HelperFunctions.GetCharasWithinRadius(__instance.pos, 3F, __instance.Chara, true, false))
                {
                    ally.HealHP(a / 2);
                }
            }

            // Dread Knight - Life Ignition reduces healing by 75%.
            if (targetConditions.ContainsKey(typeof(StanceLifeIgnition)))
            {
                a = (int)(a * 0.5F);
            }
        }

        return true;
    }

    [HarmonyPatch(nameof(Card.MakeEgg))]
    [HarmonyPrefix]
    internal static bool PromotionMod_PreventMakeEgg_Patch(Card __instance)
    {
        // The Unique Summons and Artificer Golems Should not Drop eggs.
        if (__instance.isChara &&
            __instance.Chara.trait is TraitUniqueSummon or TraitArtificerGolem)
        {
            Msg.Say("noEgg_Effect".langGame());
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(Card.Say), typeof(string), typeof(Card), typeof(string), typeof(string))]
    [HarmonyPrefix]
    internal static bool PromotionMod_Say_SkipNull(Card __instance, string lang, Card c1, string ref1 = null, string ref2 = null)
    {
        if (lang.Equals("item_drop", StringComparison.CurrentCultureIgnoreCase) && c1 is null)
        {
            return false;
        }

        return true;
    }

    [HarmonyPatch(nameof(Card.ApplyProtection))]
    [HarmonyPrefix]
    internal static bool ApplyProtection_Patch(Card __instance, long dmg, ref int mod)
    {
        if (__instance.isChara && __instance.HasCondition<ConLuna>())
        {
            // Luna will halve the damage reduction from defenses.
            mod = 50;
        }
        return true;
    }
}