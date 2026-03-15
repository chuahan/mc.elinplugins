using System.Collections.Generic;
using System.Linq;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Zone))]
public class ZonePatches
{
    public static List<int> T1SpawnableCombatSkills = new List<int>
    {
        Constants.FeatAegisId,
        Constants.FeatPaviseId,
        Constants.FeatAstraId,
        Constants.FeatLunaId,
        Constants.FeatSolId,
        Constants.FeatGaleforceId,
        Constants.FeatLethalityId,
        Constants.FeatRendHeavenId,
        Constants.FeatVengeanceId,
        Constants.FeatDeadeyeId,
        Constants.FeatVantageId
    };

    public static List<int> T2SpawnableCombatSkills = new List<int>
    {
        Constants.FeatNihilId,
        Constants.FeatAegisPlusId,
        Constants.FeatPavisePlusId,
        Constants.FeatLunaPlusId,
        Constants.FeatVantagePlusId,
        Constants.FeatMetalBreakerId
    };


    [HarmonyPatch(nameof(Zone.OnGenerateMap))]
    [HarmonyPostfix]
    internal static void ZoneOnGeneratePostfix(Zone __instance)
    {
        // Druids - If there is a druid in the party, animals and plantlife will become friendly.
        if (EClass.pc.party.members.Any(c => c.MatchesPromotion(Constants.FeatDruid)))
        {
            foreach (Chara chara in EClass._map.charas.Where(chara => !chara.IsGlobal && chara is { hostility: < Hostility.Neutral, OriginalHostility: < Hostility.Friend })
                             .Where(chara => chara.IsAnimal || chara.IsPlant))
            {
                chara.hostility = Hostility.Friend;
            }
        }
    }

    [HarmonyPatch(nameof(Zone.TryGenerateEvolved))]
    [HarmonyPostfix]
    internal static void TryGenerateEvolved_AddCombatSkill(Zone __instance, ref Chara __result, bool force, Point p)
    {
        if (EClass.pc.GetFlagValue(Constants.UnlockedEliteEnemiesFlag) > 0)
        {
            if (__instance.DangerLv > Constants.EliteEnemiesSpawnLevel)
            {
                // Do I want to have two tiers of spawning this? At some point you get the +'s
                int randomCombatSkill = T1SpawnableCombatSkills.RandomItem();
                __result.SetFeat(randomCombatSkill);
                __result.SetFlagValue(Constants.AdvancedCombatSkillFlag, randomCombatSkill);
            }
        }
    }

    /*

    [HarmonyPatch(nameof(Zone.Activate))]
    [HarmonyPrefix]
    internal static bool OnActivate_SpawnCustomeZones(Zone __instance)
    {
        if (__instance.IsRegion)
        {

        }

        return true;
    }
    */
}