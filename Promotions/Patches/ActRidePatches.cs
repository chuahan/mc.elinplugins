using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Artificer;
using PromotionMod.Stats.Machinist;
using PromotionMod.Stats.Sharpshooter;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActRide))]
public class ActRidePatches
{
    [HarmonyPatch(nameof(ActRide.Ride))]
    [HarmonyPostfix]
    public static void RidePostfix_TitanGolemPatch(ActRide __instance, Chara host, Chara t, bool parasite = false, bool talk = true)
    {
        if (!parasite && t.Evalue(Constants.FeatTitanGolem) > 0)
        {
            t.AddCondition<ConTitanProtocol>();
        }
    }
}