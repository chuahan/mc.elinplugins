using System;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities.Elementalist;
using PromotionMod.Stats;
using PromotionMod.Stats.Jenei;
using PromotionMod.Stats.WitchHunter;
using UnityEngine;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActEffect))]
public class ActEffectPatches
{
    [HarmonyPatch(nameof(ActEffect.ProcAt))]
    [HarmonyPrefix]
    internal static bool ProcAtPrefix(ActEffect __instance, EffectId id, int power, BlessedState state, ref Card cc, ref Card tc, Point tp, bool isNeg, ActRef actRef)
    {
        Chara chara = cc.Chara;
        if (cc.isChara)
        {
            // Bit Overrides
            if (id is EffectId.Funnel)
            {
                if (ActEffectPatches.HasAltSummonBits(chara))
                {
                    ActEffectPatches.SummonAltBits(chara, power, tp, actRef);
                    return false;
                }
            }

            // Elementalist - Track Spellcasts
            if (tc is { isChara: true } && chara.Evalue(Constants.FeatElementalist) > 0 && actRef.aliasEle != null && !actRef.aliasEle.IsEmpty() && (actRef.act is not ActElementalFury && actRef.act is not ActElementalExtinction))
            {
                int element = Constants.ElementIdLookup[actRef.aliasEle];
                ConElementalist? elementalist = chara.GetCondition<ConElementalist>() ?? chara.AddCondition<ConElementalist>() as ConElementalist;
                if (tc.isChara) elementalist?.AddElementalOrb(element, tc.Chara);
            }

            // Jenei - Track Spellcasts for Impact/Fire/Cold/Lightning
            if (chara.Evalue(Constants.FeatJenei) > 0 && !actRef.aliasEle.IsEmpty())
            {
                int element = Constants.ElementIdLookup[actRef.aliasEle];
                if (element is Constants.EleImpact or Constants.EleFire or Constants.EleCold or Constants.EleLightning)
                {
                    ConJenei? jenei = chara.GetCondition<ConJenei>() ?? chara.AddCondition<ConJenei>() as ConJenei;
                    jenei?.AddElement(element);
                }
            }

            if (tc.isChara)
            {
                // Witch Hunter - Magic Reflect will reflect targeted magic.
                if (tc.Chara.HasCondition<ConMagicReflect>())
                {
                    ConMagicReflect magicReflect = chara.GetCondition<ConMagicReflect>();
                    magicReflect.Mod(-1);
                    if (magicReflect.value <= 0) magicReflect.Kill();

                    // Swap the TC and CC
                    (tc, cc) = (cc, tc);
                }

                // Witch Hunter - Target gets silenced with Bane in addition to excommunication.
                if (cc.Chara.Evalue(Constants.FeatWitchHunter) > 0 && string.Equals(actRef.n1, "ConBane", StringComparison.InvariantCultureIgnoreCase))
                {
                    tc.Chara.AddCondition<ConSilence>(power);
                }
            }
        }

        return true;
    }

    private static bool HasAltSummonBits(Chara caster)
    {
        return caster.GetFlagValue(Constants.PromotionFeatFlag) is Constants.FeatElementalist or Constants.FeatBattlemage or Constants.FeatLuminary or Constants.FeatPhantom;
    }

    private static void SummonAltBits(Chara caster, int power, Point tp, ActRef actRef = default(ActRef))
    {
        Element element = Element.Create(actRef.aliasEle.IsEmpty("eleFire"), power / 10);
        int promotion = caster.GetFlagValue(Constants.PromotionFeatFlag);
        if (EClass._zone.CountMinions(caster) >= caster.MaxSummon || caster.c_uidMaster != 0)
        {
            caster.Say("summon_ally_fail", caster);
            return;
        }

        caster.Say("spell_funnel", caster, element.Name.ToLower());
        caster.PlaySound("spell_funnel");

        switch (promotion)
        {
            // Elementalists Summon multiple buffed Bits
            case Constants.FeatElementalist:
                ActEffectPatches.SummonBitInternal(Constants.NormalBitCharaId, caster, power, tp, element);
                break;
            // Battlemages Summon Shield Bits
            case Constants.FeatBattlemage:
                ActEffectPatches.SummonBitInternal(Constants.ShieldBitCharaId, caster, power, tp, element);
                break;
            // Luminaries and Spellblades Summon Sword Bits
            case Constants.FeatLuminary:
            case Constants.FeatSpellblade:
                ActEffectPatches.SummonBitInternal(Constants.SwordBitCharaId, caster, power, tp, element);
                break;
            // Phantoms Summon Phantom Bits
            case Constants.FeatPhantom:
                ActEffectPatches.SummonBitInternal(Constants.PhantomBitCharaId, caster, power, tp, element);
                break;
            default:
                return;
        }
    }

    private static void SummonBitInternal(string type, Chara caster, int power, Point tp, Element element)
    {
        // If it's the normal bit, which elementalists summon, they summon more of them and they spawn Boosted.
        int num = 2;
        bool addBuffs = false;
        if (type == Constants.NormalBitCharaId)
        {
            num = Math.Max(num, Mathf.Clamp(power / 100, 1, 5) + (power >= 100 ? EClass.rnd(2) : 0));
            addBuffs = true;
        }

        int levelOverride = power / 15;
        if (caster.IsPC) levelOverride = Math.Max(EClass.player.stats.deepest, levelOverride);

        for (int i = 0; i < num; i++)
        {
            Chara summonedBit = CharaGen.Create(type);
            summonedBit.SetMainElement(element.source.alias, element.Value, true);
            summonedBit.SetSummon(20 + power / 20 + EClass.rnd(10));
            summonedBit.SetLv(levelOverride);
            summonedBit.interest = 0;
            EClass._zone.AddCard(summonedBit, tp.GetNearestPoint(false, false));
            summonedBit.PlayEffect("teleport");
            summonedBit.MakeMinion(caster);
            if (addBuffs)
            {
                summonedBit.AddCondition<ConProtection>(power);
                summonedBit.AddCondition<ConBoost>();
            }
        }
    }
}