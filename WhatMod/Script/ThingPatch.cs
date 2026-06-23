using HarmonyLib;
namespace WhatMod;

[HarmonyPatch(typeof(Thing))]
public class ThingPatch
{
    [HarmonyPatch(typeof(Thing), nameof(Thing.WriteNote))]
    [HarmonyPostfix]
    internal static void WhatMod_Thing_WriteNotePatch(Thing __instance, UINote n, IInspect.NoteMode mode, Recipe recipe)
    {
        ModPackage modPackage = ModUtil.FindSourceRowPackage(__instance.source);
        if (modPackage == null) return;
        n.AddText(("<size=12>" + "whatmod_addedby".lang(modPackage.title) + "</size>").TagColor(FontColor.Flavor));
    }
}