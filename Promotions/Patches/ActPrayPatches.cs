using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
using PromotionMod.Stats.Sentinel;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActPray))]
public class ActPrayPatches
{
    [HarmonyPatch(nameof(ActPray.Pray))]
    [HarmonyPostfix]
    public static void PrayPatch(ActPray __instance, Chara c, bool passive)
    {
        // Saint - If the player is a Saint, or has (a) Saint ally(allies), when praying, add Protection.
        List<Chara> alliedSaints = EClass.pc.party.members.Where(x => x.Evalue(Constants.FeatSaint) > 0).ToList();
        if (c.IsPC && (c.Evalue(Constants.FeatSaint) > 0 || alliedSaints.Count != 0))
        {
            // Add Protection to entire party based on Faith of all Saints.
            int totalFaith = 0;
            if (c.Evalue(Constants.FeatSaint) > 0) totalFaith += c.Evalue(306);
            totalFaith += alliedSaints.Sum(alliedSaint => alliedSaint.Evalue(306));
            foreach (Chara target in c.party.members)
            {
                target.AddCondition<ConProtection>(ConProtection.CalcProtectionAmount(totalFaith));
            }
        }
    }
}