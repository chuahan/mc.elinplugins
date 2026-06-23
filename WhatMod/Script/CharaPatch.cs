using HarmonyLib;
namespace WhatMod;

[HarmonyPatch(typeof(Chara))]
public class CharaPatch
{
    [HarmonyPatch(nameof(Chara.GetHoverText))]
    [HarmonyPostfix]
    internal static void WhatMod_Chara_GetHoverTextPatch(Chara __instance, ref string __result)
    {
        ModPackage modPackage = ModUtil.FindSourceRowPackage(__instance.source);
        if (modPackage == null) return;
        __result += (" <size=12>" + "whatmod_addedby".lang(modPackage.title) + "</size>").TagColor(FontColor.Flavor);
    }
}