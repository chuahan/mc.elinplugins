using System.Collections.Generic;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(InvOwner))]
internal class InvOwnerPatches : EClass
{
    [HarmonyPatch(typeof(InvOwner), nameof(InvOwner.ListInteractions), typeof(ButtonGrid), typeof(bool))]
    [HarmonyPostfix]
    internal static void PromoMod_InvOwnerPatches_OnListInteractions_Postfix(InvOwner __instance, ref InvOwner.ListInteraction __result, ButtonGrid b, bool context)
    {
        Thing t = b.card.Thing;
        global::Trait trait = t.trait;
        if (context)
        {
            if (trait is TraitSpellbook && __instance.owner.IsPC && SpellbookConversionManager.CanConvertSpellbook(EClass.pc, (TraitSpellbook)trait))
            {
                __result.Add("convertSpellbook".lang(), 299, delegate
                {
                    if (SpellbookConversionManager.TryConvert(EClass.pc, (TraitSpellbook)trait))
                    {
                        pc.Say("convertedSpellbook".langGame(pc.NameSimple, sources.elements.map[trait.owner.refVal].name));
                        LayerInventory.SetDirty(t);
                        SE.ClickOk();    
                    }
                    else
                    {
                        pc.Say("convertSpellbookFailed".langGame());
                    };
                });
            }
        }
    }
}