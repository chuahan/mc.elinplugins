using BardMod.Source;
using HarmonyLib;

namespace BardMod.Patches;

[HarmonyPatch(typeof(Thing))]
internal class BardThingPatches : EClass
{
    // Patch to add the option to mark as selected instrument.
    [HarmonyPatch(typeof(Thing), nameof(Thing.WriteNote))]
    [HarmonyPostfix]
    internal static void WriteNote(Thing __instance, UINote n, IInspect.NoteMode mode, Recipe recipe)
    {
        if (__instance.trait is not TraitToolBard) return;
        if (__instance.elements != null)
        {
            __instance.elements.AddNote(n, (Element e) => e.IsFlag, null, ElementContainer.NoteMode.BonusTrait,
                addRaceFeat: false, delegate(Element e, string s)
                {
                    string text = "altEnc".lang("", e.FullName, "");
                    text +=(" <size=12>" + e.source.GetText("textPhase") + "</size>").TagColor(FontColor.Good);
                    return text;
                });
        }
    }
}