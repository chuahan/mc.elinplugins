using System.Collections.Generic;
using HarmonyLib;
using NierMod.Stats;

namespace NierMod.Patches
{
    [HarmonyPatch(typeof(ActEffect))]
    internal class ActEffectPatches : EClass
    {
       
        [HarmonyPatch(nameof(ActEffect.ProcAt))]
        [HarmonyPrefix]
        internal static bool OnHealingCasted(EffectId id, int power, BlessedState state, Card cc, Card tc, Point tp, bool isNeg, ActRef actRef = default(ActRef))
        {
            List<EffectId> HealingEffectIds = new List<EffectId>
            {
                EffectId.HealComplete,
                EffectId.Heal,
                EffectId.JureHeal,
                EffectId.RemedyJure,
            };

            if (HealingEffectIds.Contains(id))
            {
                if (tc.isChara)
                {
                    if (tc.Chara.HasCondition<ConUnfinishedBusiness>())
                    {
                        tc.Say("debuffUnfinishedBusiness".lang());
                        return false;
                    }
                }
            }

            return true;
        }
        
    }
}