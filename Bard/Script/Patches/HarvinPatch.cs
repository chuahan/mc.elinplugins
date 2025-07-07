using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BardMod.Common;
using HarmonyLib;

namespace BardMod.Patches;

[HarmonyPatch(typeof(ActEffect))]
internal class HarvinPatch : EClass
{
    // Yes this is a patch to stop Harvins from growing taller.
    [HarmonyPatch(nameof(ActEffect.ProcAt))]
    [HarmonyPrefix]
    internal static bool OnMilkEffect(EffectId id, int power, BlessedState state, Card cc, Card tc, Point tp,
        bool isNeg, ActRef actRef = default(ActRef))
    {
        bool blessed = state >= BlessedState.Blessed;
        if (id == EffectId.DrinkMilk && blessed)
        {
            Chara TC = tc.Chara;
            if (TC.Evalue(Constants.FeatHarvin) > 0)
            {
                Msg.Say("wishFail");
                return false;
            }
        }
        return true;
    }
}