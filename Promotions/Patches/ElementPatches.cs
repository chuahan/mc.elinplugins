using System;
using System.Reflection;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.Battlemage;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Element))]
public class ElementPatches
{
    [HarmonyPatch(nameof(Element.GetCost))]
    [HarmonyPostfix]
    internal static void GetCostPatch(Element __instance, ref Act.Cost __result, Chara c)
    {
        if (c.MatchesPromotion(Constants.FeatBattlemage) &&
            __instance is Spell &&
            __result.type == Act.CostType.MP)
        {
            // Focus Stance increases costs based on current mana.
            StanceManaFocus focusCon = c.GetCondition<StanceManaFocus>();
            if (focusCon != null && c.mana.value > 0)
            {
                __result.cost += (int)(c.mana.value * 0.15F);
            }
        }
    }

    [HarmonyPatch(typeof(Element.BonusInfo))]
    [HarmonyPatch("WriteNote")]
    [HarmonyPatch(new Type[]
    {
    })]
    [HarmonyPostfix]
    internal static void WriteNote_Necromancer_Druid_Patches(MethodBase __originalMethod, Element.BonusInfo __instance)
    {
        // Necromancers and Druids will both apply a Hardware boost like upgrade to Undead and Plant allies respectively.
        // Necromancer - Lord of the Dead
        // Druid - Speaker for Nature
        if (__instance.ele.id is SKILL.DV or SKILL.PV or SKILL.SPD &&
            (__instance.ele.owner.Chara.IsUndead || __instance.ele.owner.Chara.IsPlant))
        {
            bool isEnemy = __instance.ele.owner.Chara.IsHostile(EClass.pc);

            int undeadBoost = 0;
            int plantBoost = 0;
            // Check for all characters on the map for any Necromancers or Druids.
            foreach (Chara member in EClass._map.charas)
            {
                if (isEnemy && member.IsHostile(EClass.pc) || !isEnemy && !member.IsHostile(EClass.pc))
                {
                    if (member.MatchesPromotion(Constants.FeatNecromancer))
                    {
                        undeadBoost++;
                    }

                    if (member.MatchesPromotion(Constants.FeatDruid))
                    {
                        plantBoost++;
                    }
                }
            }

            // Cap these boosts at 10.
            undeadBoost = Math.Min(10, undeadBoost);
            plantBoost = Math.Min(10, plantBoost);

            if (undeadBoost > 0 && __instance.ele.owner.Chara.IsUndead)
            {
                switch (__instance.ele.id)
                {
                    case SKILL.DV: // DV
                    case SKILL.PV: // PV
                        __instance.AddFix(undeadBoost * 10, "necromancer_lod_boost".lang());
                        return;
                    case SKILL.SPD: // SPD
                        __instance.AddFix(undeadBoost * 20, "necromancer_lod_boost".lang());
                        return;
                }
            }

            if (plantBoost > 0 && __instance.ele.owner.Chara.IsPlant)
            {
                switch (__instance.ele.id)
                {
                    case SKILL.DV: // DV
                    case SKILL.PV: // PV
                        __instance.AddFix(plantBoost * 10, "druid_son_boost".lang());
                        return;
                    case SKILL.SPD: // SPD
                        __instance.AddFix(plantBoost * 20, "druid_son_boost".lang());
                        return;
                }
            }
        }
    }
}