using System;
using HarmonyLib;
namespace WhatMod;

[HarmonyPatch(typeof(Element))]
public class ElementPatch
{
    [HarmonyPatch(nameof(Element._WriteNote), typeof(UINote), typeof(ElementContainer), typeof(Action<UINote>), typeof(bool), typeof(bool))]
    [HarmonyPostfix]
    internal static void WhatMod_Element_WriteNotePatch(Element __instance, UINote n, ElementContainer owner, Action<UINote> onWriteNote, bool isRef, bool addHeader = true)
    {
        ModPackage modPackage = ModUtil.FindSourceRowPackage(__instance.source);
        if (modPackage == null) return;
        n.AddText(("<size=12>" + "whatmod_addedby".lang(modPackage.title) + "</size>").TagColor(FontColor.Flavor));
    }
}