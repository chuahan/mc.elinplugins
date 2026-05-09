using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities;
using PromotionMod.Elements.PromotionAbilities.Elementalist;
using PromotionMod.Stats;
using PromotionMod.Stats.Battlemage;
using PromotionMod.Stats.Jenei;
using UnityEngine;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Act))]
public class ActPatches
{
    [HarmonyPatch(nameof(Act.ValidatePerform))]
    [HarmonyPostfix]
    internal static void PromotionMod_ValidatePerformPatch_SpellcastCounter(Act __instance, ref bool __result, Chara _cc, Card _tc, Point _tp)
    {
        string spellType = __instance.act.source.aliasRef;
        if (__instance.act is Spell && _cc != null && __result && !string.IsNullOrEmpty(spellType))
        {
            // Elementalist - Track Spellcasts
            if (_cc.HasElement(Constants.FeatElementalist) &&
                __instance.act is not ActElementalFury &&
                __instance.act is not ActElementalExtinction)
            {
                if (Constants.ElementAliasLookup.ContainsValue(spellType))
                {
                    int element = Constants.ElementIdLookup[spellType];
                    ConElementalist? elementalist = _cc.GetCondition<ConElementalist>() ?? _cc.AddCondition<ConElementalist>() as ConElementalist;
                    if (_tc is { isChara: true })
                    {
                        elementalist?.AddElementalOrb(element, _tc.Chara.IsHostile(_cc) ? _tc.Chara : null);
                    }
                }
            }

            // Jenei - Track Spellcasts for Impact/Fire/Cold/Lightning
            if (_cc.HasElement(Constants.FeatJenei))
            {
                if (Constants.ElementAliasLookup.ContainsValue(spellType))
                {
                    int element = Constants.ElementIdLookup[spellType];
                    if (element is Constants.EleImpact or Constants.EleFire or Constants.EleCold or Constants.EleLightning)
                    {
                        ConJenei? jenei = _cc.GetCondition<ConJenei>() ?? _cc.AddCondition<ConJenei>() as ConJenei;
                        jenei?.AddElement(element);
                    }
                }
            }
        }
    }
}