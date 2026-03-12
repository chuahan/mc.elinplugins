using System.Collections.Generic;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.Battlemage;
using UnityEngine;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ElementContainerCard))]
public class ElementContainerCardPatches
{
    [HarmonyPatch(nameof(ElementContainerCard.ValueBonus))]
    [HarmonyPostfix]
    internal static void ValueBonus(ElementContainerCard __instance, ref int __result, Element e)
    {
        if (EClass.game == null)
        {
            return;
        }

        // Necromancers and Druids will both apply a Hardware boost like upgrade to Undead and Plant allies respectively.
        // Necromancer - Lord of the Dead
        // Druid - Speaker for Nature
        // 64 is DV
        // 65 is PV
        // 79 is Speed
        
        // Boost these three evalues only.
        if (e.id is 64 or 65 or 79 &&
            __instance.owner.isChara && 
            (__instance.owner.Chara.IsUndead || __instance.owner.Chara.IsPlant))
        {
            bool isEnemy = __instance.owner.Chara.IsHostile(EClass.pc);
            
            int undeadBoost = 0;
            int plantBoost = 0;
            // Check for all characters on the map for any Necromancers or Druids.
            foreach (Chara member in EClass._map.charas)
            {
                if (isEnemy && member.IsHostile(EClass.pc))
                {
                    if (member.MatchesPromotion(Constants.FeatNecromancer))
                    {
                        undeadBoost++;
                    }    
                } else if (!isEnemy && !member.IsHostile(EClass.pc))
                {
                    if (member.MatchesPromotion(Constants.FeatDruid))
                    {
                        plantBoost++;
                    }
                }
                
            }

            if (undeadBoost > 0 && __instance.Chara.IsUndead)
            {
                switch (e.id)
                {
                    case 64: // DV
                    case 65: // PV
                        __result = (int)(__result * (1 + 0.1F * undeadBoost));
                        return;
                    case 79: // SPD
                        __result = (int)(__result * (1 + 0.2F * undeadBoost));
                        return;
                }
            }

            if (plantBoost > 0 && __instance.Chara.IsPlant)
            {
                switch (e.id)
                {
                    case 64: // DV
                    case 65: // PV
                        __result = (int)(__result * (1 + 0.1F * plantBoost));
                        return;
                    case 79: // SPD
                        __result = (int)(__result * (1 + 0.2F * plantBoost));
                        return;
                }
            }
        }
    }
}