using System;
using HarmonyLib;
using PromotionMod.Common;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ElementContainerCard))]
public class ElementContainerCardPatches
{
    [HarmonyPatch(nameof(ElementContainerCard.ValueBonus))]
    [HarmonyPostfix]
    internal static void ValueBonus(ElementContainerCard __instance, ref int __result, Element e)
    {
        if (EClass.game == null || e == null)
        {
            return;
        }

        // TODO: Should I make this apply to themselves if they match the race tag?
        // Necromancers and Druids will both apply a Hardware boost like upgrade to Undead and Plant allies respectively.
        // Necromancer - Lord of the Dead
        // Druid - Speaker for Nature
        // Boost these three evalues only.
        if (e.id is SKILL.DV or SKILL.PV or SKILL.SPD &&
            __instance.owner is { isChara: true } &&
            (__instance.owner.Chara.IsUndead || __instance.owner.Chara.IsPlant) &&
            EClass.pc.currentZone != null &&
            EClass.pc.currentZone.map != null)
        {
            bool isEnemy = __instance.owner.Chara.IsHostile(EClass.pc);

            int undeadBoost = 0;
            int plantBoost = 0;
            // Check for all allied characters to the target on the map for any Necromancers or Druids.
            foreach (Chara member in EClass.pc.currentZone.map.charas)
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

            if (undeadBoost > 0 && __instance.Chara.IsUndead)
            {
                int baseValue = e.ValueWithoutLink + e.vLink;
                switch (e.id)
                {
                    case SKILL.DV: // DV
                    case SKILL.PV: // PV
                        __result = (int)(baseValue * (1 + 0.1F * undeadBoost));
                        return;
                    case SKILL.SPD: // SPD
                        __result = (int)(baseValue * (1 + 0.2F * undeadBoost));
                        return;
                }
            }

            if (plantBoost > 0 && __instance.Chara.IsPlant)
            {
                int baseValue = e.ValueWithoutLink + e.vLink;
                switch (e.id)
                {
                    case SKILL.DV: // DV
                    case SKILL.PV: // PV
                        __result = (int)(baseValue * (1 + 0.1F * plantBoost));
                        return;
                    case SKILL.SPD: // SPD
                        __result = (int)(baseValue * (1 + 0.2F * plantBoost));
                        return;
                }
            }
        }
    }
}