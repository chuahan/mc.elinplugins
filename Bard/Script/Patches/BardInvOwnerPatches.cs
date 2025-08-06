using System.Collections.Generic;
using BardMod.Common.HelperFunctions;
using BardMod.Traits;
using Cwl.Helper.Extensions;
using HarmonyLib;
namespace BardMod.Patches;

[HarmonyPatch(typeof(InvOwner))]
internal class BardInvOwnerPatches : EClass
{
    // Patch to add the option to mark as selected instrument.
    [HarmonyPatch(typeof(InvOwner), nameof(InvOwner.ListInteractions), typeof(ButtonGrid), typeof(bool))]
    [HarmonyPostfix]
    internal static void OnListInteractions(InvOwner __instance, ref InvOwner.ListInteraction __result, ButtonGrid b, bool context)
    {
        Thing t = b.card.Thing;
        Trait trait = t.trait;
        if (context)
        {
            if (trait is TraitToolBard && __instance.owner.IsPC)
            {
                TraitToolBard tool = trait as TraitToolBard;
                __result.Add(tool.IsSelectedInstrument ? "unequip_bardInstrument".lang() : "equip_bardInstrument".lang(), 299, delegate
                {
                    bool newState = !tool.IsSelectedInstrument;
                    if (newState)
                    {
                        List<Thing> allInstruments = HelperFunctions.GetAllInstruments();
                        foreach (Thing instrument in allInstruments)
                        {
                            if (instrument != t)
                            {
                                if ((instrument.trait as TraitToolBard).IsSelectedInstrument)
                                {
                                    (instrument.trait as TraitToolBard).owner.SetFlagValue(TraitToolBard.IsSelectedInstrumentFlag, 0);
                                    pc.Say("hintUnequippedBardInstrument".lang(pc.NameSimple, instrument.Name));
                                }
                            }

                        }
                        pc.Say("hintEquippedBardInstrument".lang(pc.NameSimple, t.Name));
                    }

                    tool.owner.SetFlagValue(TraitToolBard.IsSelectedInstrumentFlag, newState ? 1 : 0);
                    LayerInventory.SetDirty(t);
                    SE.ClickOk();
                });
            }
        }
    }
}