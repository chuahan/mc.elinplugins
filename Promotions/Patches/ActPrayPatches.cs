using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActPray))]
public class ActPrayPatches
{
    [HarmonyPatch(nameof(ActPray.Pray))]
    [HarmonyPostfix]
    public static void PrayPatch(ActPray __instance, Chara c, bool passive)
    {
        // Saint - If the player is a Saint/War Cleric, or has (a) Saint/War Cleric ally(allies), when praying, add Protection.
        List<Chara> allyAdvancedPray = EClass.pc.party.members.Where(x => x.Evalue(Constants.FeatSaint) > 0 || x.Evalue(Constants.FeatWarCleric) > 0).ToList();
        if (c.IsPC && (c.Evalue(Constants.FeatSaint) > 0 || c.Evalue(Constants.FeatWarCleric) > 0 || allyAdvancedPray.Count != 0))
        {
            // Add Protection to entire party based on Faith of all Saints and War Clerics.
            int totalFaith = 0;
            if (c.Evalue(Constants.FeatSaint) > 0 || c.Evalue(Constants.FeatWarCleric) > 0) totalFaith += c.Evalue(Constants.FaithId);
            totalFaith += allyAdvancedPray.Sum(alliedSaint => alliedSaint.Evalue(Constants.FaithId));
            // Faith is multiplied by 10.
            totalFaith = HelperFunctions.SafeMultiplier(totalFaith, 10);
            foreach (Chara target in c.party.members)
            {
                target.AddCondition<ConProtection>(totalFaith);
            }
        }
    }
}