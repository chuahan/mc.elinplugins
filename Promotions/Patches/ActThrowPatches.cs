using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PromotionMod.Source.QuestEventSystem.QuestTypes;
using PromotionMod.Stats;
using PromotionMod.Trait.QuestTraits;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActThrow))]
public class ActThrowPatches
{
    [HarmonyPatch(nameof(ActThrow.CanPerform))]
    [HarmonyPrefix]
    public static bool CanPerformPatch(ref bool __result)
    {
        if (Act.CC.HasCondition<ConDisable>())
        {
            __result = false;
            return false;
        }
        return true;

    }

    [HarmonyPatch(nameof(ActThrow.CanThrow))]
    [HarmonyPostfix]
    public static void CanThrow_SnagballEvaluation_Patch(ActThrow __instance, ref bool __result, Chara c, Thing t, Card target, Point p)
    {
        if (t.trait is not TraitSnagBall) return;
        List<QuestCovertOpAbduct> abductionQuests = EClass.game.quests.list.Where(q => q is QuestCovertOpAbduct).Cast<QuestCovertOpAbduct>().ToList();
        // If it's a Snagball and is an abduction target, you can throw it; else don't allow it.
        if (abductionQuests.Any(abductionQuest => target.uid == abductionQuest.targetCharaUID && target.hp <= target.MaxHP / 10))
        {
            return;
        }
        else
        {
            __result = false;
        }
    }
}