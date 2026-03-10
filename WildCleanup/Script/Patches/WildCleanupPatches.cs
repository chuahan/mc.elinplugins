
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace WildCleanup.Patches
{
    [HarmonyPatch(typeof(FactionBranch))]
    internal class WildCleanupPatches : EClass
    {
        [HarmonyPatch(nameof(FactionBranch.OnActivateZone))]
        [HarmonyPostfix]
        internal static void OnActivateZonePatch(FactionBranch __instance)
        {
            if (EClass.pc.currentZone.IsPCFactionOrTent)
            {
                List<Chara> curZone = EClass._zone.map.charas.Where(chara => !chara.IsPCFactionOrMinion).ToList();
                foreach (Chara t in curZone)
                {
                    if (t.id is "hound" or "triceratops")
                    {
                        WildCleanup.Log("================");
                        WildCleanup.Log(t.id);
                        WildCleanup.Log(t.pos.x);
                        WildCleanup.Log(t.pos.z);
                        WildCleanup.Log(t.uid);
                    }
                }
            }
        }
    }
}
