using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.Headhunter;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Card))]
public class CardPatches
{
    [HarmonyPatch(nameof(Card.ModExp))]
    [HarmonyPrefix]
    internal static bool AdventurerDoubleExp(Card __instance, int ele, ref int a)
    {
        if (__instance.isChara)
        {
            if (__instance.Chara.IsPCFaction && EClass.pc.Evalue(Constants.FeatAdventurer) > 0)
            {
                // Adventurers increase the exp gain by 50%
                a = (int)(a * 1.5F);
            } 
        }
        return true;
    }
    
    [HarmonyPatch(typeof(Card), "get_PV")]
    [HarmonyPostfix]
    static void BattlemagePVBoostPatch(Card __instance, ref int __result)
    {
        if (__instance.isChara && __instance.Chara.Evalue(Constants.FeatBattlemage) > 0)
        {
            int manaValue = __instance.Chara.mana.max / 5;
            __result += manaValue;
        }
    }

    [HarmonyPatch(nameof(Card.Die))]
    [HarmonyPrefix]
    internal static bool OnDieOverloads(Card __instance, Element e, Card origin, AttackSource attackSource)
    {
        // Berserker - Heal on Kill
        if (__instance.isChara && origin.isChara && origin.Chara.Evalue(Constants.FeatBerserker) > 0)
        {
            int healAmount = (int)(origin.Chara.MaxHP * .25F);
            origin.Say("berserker_revel".langGame(origin.Chara.NameSimple));
            origin.Chara.HealHP(healAmount);
        }
        
        // Headhunter - Gain Headhunter stacks on Kill.
        if (__instance.isChara && origin.isChara && origin.Chara.Evalue(Constants.FeatHeadhunter) > 0)
        {
            if (!origin.Chara.HasCondition<ConHeadhunter>())
            {
                origin.Chara.AddCondition<ConHeadhunter>(1);
            }
            else
            {
                int newStacks = origin.Chara.GetCondition<ConHeadhunter>().power + 1;
                origin.Chara.AddCondition<ConHeadhunter>(newStacks);
            }
        }

        return true;
    }
}

