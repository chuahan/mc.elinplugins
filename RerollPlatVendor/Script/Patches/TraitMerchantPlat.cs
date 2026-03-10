
using HarmonyLib;

namespace RerollPlatMerchant.Patches
{
    [HarmonyPatch(typeof(Trait))]
    internal class TraitPatches : EClass
    {
        [HarmonyPatch(nameof(Trait.CostRerollShop))]
        [HarmonyPatch(MethodType.Getter)]
        [HarmonyPrefix]
        internal static bool CostRerollShopOverride(Trait __instance, ref int __result)
        {
            if (__instance is not TraitMerchantPlat) return true;
            __result = RerollPlatMerchantConfig.RerollAmount != null ? RerollPlatMerchantConfig.RerollAmount.Value : 5;
            return false;

        }
    }
}
