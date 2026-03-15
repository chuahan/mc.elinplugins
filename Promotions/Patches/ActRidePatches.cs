using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.Artificer;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActRide))]
public class ActRidePatches
{
    [HarmonyPatch(nameof(ActRide.Ride))]
    [HarmonyPostfix]
    public static void RidePostfix_TitanGolemPatch(ActRide __instance, Chara host, Chara t, bool parasite = false, bool talk = true)
    {
        if (!parasite && t.HasElement(Constants.FeatTitanGolemId))
        {
            t.AddCondition<ConTitanProtocol>();
        }
    }
}