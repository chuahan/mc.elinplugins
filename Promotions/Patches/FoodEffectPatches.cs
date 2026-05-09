using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PromotionMod.Common;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(FoodEffect))]
public class FoodEffectPatches
{
    [HarmonyPatch(nameof(FoodEffect.Proc))]
    [HarmonyTranspiler]
    internal static IEnumerable<CodeInstruction> PromotionMod_AlrauneNutritionPenalty_FoodEffectPatches(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        Type? displayClassType = typeof(FoodEffect)
                .GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Public)
                .FirstOrDefault(t =>
                        t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                .Any(f => f.FieldType == typeof(Chara) && f.Name == "c"));
        if (displayClassType == null) throw new Exception("FoodEffect::Why was this removed?");
        FieldInfo? charaField = displayClassType.GetField("c",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        MethodInfo nutritionModifier = AccessTools.Method(
            typeof(FoodEffectPatches),
            nameof(FoodEffectPatches.ApplyNutritionModifier),
            new[]
            {
                typeof(float),
                typeof(Chara)
            });

        List<CodeInstruction> instructionList = instructions.ToList();
        CodeMatcher matcher = new CodeMatcher(instructionList, il);
        matcher.MatchForward(
            false,
            new CodeMatch(OpCodes.Ldloc_3),
            new CodeMatch(OpCodes.Ldc_R4, 3f),
            new CodeMatch(OpCodes.Mul),
            new CodeMatch(OpCodes.Stloc_3)
        );

        // We want to inject this function before the NPC 3x Multiplier.
        int insertIndex = matcher.Pos - 3;

        List<CodeInstruction> injected = new List<CodeInstruction>
        {
            new CodeInstruction(OpCodes.Ldloc_3), // Load Nutrition (num2)
            new CodeInstruction(OpCodes.Ldloc_0), // Load Chara Display Class
            new CodeInstruction(OpCodes.Ldfld, charaField), // Store Chara.
            new CodeInstruction(OpCodes.Call, nutritionModifier),
            new CodeInstruction(OpCodes.Stloc_3)
        };

        instructionList.InsertRange(insertIndex, injected);
        return instructionList;
    }

    internal static float ApplyNutritionModifier(float nutrition, Chara consumer)
    {
        //Msg.Nerun($"Incoming Nutrition: {nutrition}");
        if (!consumer.HasElement(Constants.FeatAlraune)) return nutrition;
        //Msg.Nerun("Modifying nutrition for Alraune.");
        return nutrition * 0.3F;
    }
}