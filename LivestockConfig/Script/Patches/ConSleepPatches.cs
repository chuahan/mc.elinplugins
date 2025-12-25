using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
namespace LivestockTweaks.Patches;

[HarmonyPatch(typeof(ConSleep))]
internal class ConSleepPatches
{
    [HarmonyPatch(nameof(ConSleep.Tick))]
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> SleepBesideTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> il = new List<CodeInstruction>(instructions);
        CodeMatcher matcher = new CodeMatcher(il, generator);

        MethodInfo? overrideMethod = AccessTools.Method(typeof(ConSleepPatches), nameof(SleepBesideOverride));
        MethodInfo? moveImmediate = AccessTools.Method(typeof(Card), nameof(Card.MoveImmediate),
            new[] {
                typeof(Point),
                typeof(bool),
                typeof(bool)
            });
        
        matcher.MatchForward(false,
            new CodeMatch(ci => ci.opcode == OpCodes.Callvirt && ci.operand as MethodInfo == moveImmediate));
        int callPos = matcher.Pos;
        
        int branchPos = -1;
        for (int i = callPos - 1; i >= 0; i--)
        {
            var op = il[i].opcode;
            if (op == OpCodes.Brfalse || op == OpCodes.Brfalse_S || op == OpCodes.Brtrue || op == OpCodes.Brtrue_S)
            {
                branchPos = i;
                break;
            }
        }
        
        CodeInstruction? st = il[branchPos - 2];
        CodeInstruction? ld = il[branchPos - 1];

        int flagIndex =
                st.operand is LocalBuilder lb ? lb.LocalIndex :
                st.operand is int n ? n :
                ld.operand is LocalBuilder lb2 ? lb2.LocalIndex :
                ld.operand is int n2 ? n2 :
                -1;
        
        matcher.Start().Advance(branchPos - 1);
        
        OpCode ldOp = flagIndex <= byte.MaxValue ? OpCodes.Ldloc_S : OpCodes.Ldloc;
        OpCode stOp = flagIndex <= byte.MaxValue ? OpCodes.Stloc_S : OpCodes.Stloc;

        matcher.Insert(
            new CodeInstruction(ldOp, flagIndex),
            new CodeInstruction(OpCodes.Call, overrideMethod),
            new CodeInstruction(stOp, flagIndex)
        );
        return matcher.InstructionEnumeration();
    }

    static bool SleepBesideOverride(bool input)
    {
        // Play Sound here.
        if (input && LivestockTweaksConfig.NoSleepBesideMe?.Value == true)
        {
            return false;
        }
        return input;
    }
}
