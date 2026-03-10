using System;
using System.Collections.Generic;
using System.Linq;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(RecipeManager))]
public class RecipeManagerPatches
{
    [HarmonyPatch(nameof(RecipeManager.ListSources))]
    [HarmonyPostfix]
    public static void RidePostfix_TitanGolemPatch(RecipeManager __instance, ref List<RecipeSource> __result, Thing factory, List<RecipeSource> newRecipes)
    {
        if (factory.id.Equals("artificer_workbench", StringComparison.InvariantCulture))
        {
            // If a golem has been crafted, remove this recipe from the craftable list.
            // Do I want to remove the golem parts too?
            if (EClass.pc.GetFlagValue(Constants.ArtificerGolemCreated) > 0 )
            {
                RecipeSource golemRecipe = __result.FirstOrDefault(r => r.id == "artificer_golem");
                if (golemRecipe != null) __result.Remove(golemRecipe);
            }
        }
    }
}