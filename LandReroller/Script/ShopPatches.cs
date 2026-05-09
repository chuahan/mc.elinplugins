using HarmonyLib;
namespace LandReroller;

/// <summary>
/// Lotta hacky stuff to make it only pop once... 
/// </summary>
[HarmonyPatch(typeof(Trait))]
public class ShopPatches
{
    [HarmonyPatch(nameof(Trait.OnBarter))]
    [HarmonyPrefix]
    internal static bool LandReroller_OnBarterRestockFlag(Trait __instance)
    {
        if (!EClass.world.date.IsExpired(__instance.owner.c_dateStockExpire) || (__instance.RestockDay < 0 && __instance.owner.isRestocking))
        {
            return true;
        }

        // If there's a restock incoming, set a flag on the character, then return true so it continues the OnBarter.
        if (__instance.ShopType == ShopType.Loytel)
        {
            __instance.owner.mapInt[68256] = 1;
        }

        return true;
    }
    
    [HarmonyPatch(nameof(Trait.OnBarter))]
    [HarmonyPostfix]
    internal static void LandReroller_OnBarterAddHammers(Trait __instance)
    {
        // Since this stuff runs as a Postfix, I want to basically catch it as it just expired.
        if (__instance.ShopType == ShopType.Loytel && __instance.owner.mapInt.ContainsKey(68256))
        {
            Thing t = __instance.owner.things.Find("chest_merchant");
            if (__instance.owner.things.Find("landhammer_lesser") == null) t.Add("landhammer_lesser", 5, 0);
            if (__instance.owner.things.Find("landhammer_greater") == null) t.Add("landhammer_greater", 1, 0);
            __instance.owner.mapInt.Remove(68256);
        }
    }
}