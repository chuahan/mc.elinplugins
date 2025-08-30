using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActEffect))]
public class ActEffectPatches
{
    [HarmonyPatch(nameof(ActEffect.ProcAt))]
    [HarmonyPrefix]
    internal static bool OverrideProcAt(EffectId id, int power, BlessedState state, Card cc, Card tc, Point tp, bool isNeg, ActRef actRef = default(ActRef))
    {
        // Bit Overrides
        if (id is EffectId.Funnel)
        {
            if (cc.isChara && HasAltSummonBits(cc.Chara))
            {
                SummonAltBits(cc.Chara, power, tp, actRef);
                return false;
            }
        }

        return true;
    }

    public static bool HasAltSummonBits(Chara caster)
    {
        caster.Say("DEBUG::HasAltSummonBits");
        if (PromotionMod.Debug) return true;
        return caster.GetFlagValue(Constants.PromotionFeatFlag) is Constants.FeatElementalist or Constants.FeatBattlemage or Constants.FeatLuminary or Constants.FeatPhantom;
    }

    public static void SummonAltBits(Chara caster, int power, Point tp, ActRef actRef = default(ActRef))
    {
        if (PromotionMod.Debug) caster.Say("DEBUG::SummonBits");
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
                SummonBitInternal("bit", caster, power, tp, element);
                break;
            // Battlemages Summon Shield Bits
            case Constants.FeatBattlemage:
                SummonBitInternal("shield_bit", caster, power, tp, element);
                break;
            // Luminaries Summon Sword Bits
            case Constants.FeatLuminary:
                SummonBitInternal("sword_bit", caster, power, tp, element);
                break;
            // Phantoms Summon Phantom Bits
            case Constants.FeatPhantom:
                SummonBitInternal("phantom_bit", caster, power, tp, element);
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
        if (type == "bit")
        {
            num = Math.Max(num, Mathf.Clamp(power / 100, 1, 5) + ((power >= 100) ? EClass.rnd(2) : 0));
            addBuffs = true;
        }
        
        int levelOverride = power / 15;
        if (caster.IsPC) levelOverride = Math.Max(EClass.player.stats.deepest, levelOverride);
        
        for (int i = 0; i < num; i++)
        {
            Chara summonedBit = CharaGen.Create(type);
            summonedBit.SetMainElement(element.source.alias, element.Value, elemental: true);
            summonedBit.SetSummon(20 + power / 20 + EClass.rnd(10));
            summonedBit.SetLv(levelOverride);
            summonedBit.interest = 0;
            EClass._zone.AddCard(summonedBit, tp.GetNearestPoint(allowBlock: false, allowChara: false));
            summonedBit.PlayEffect("teleport");
            summonedBit.MakeMinion(caster);
            if (addBuffs)
            {
                summonedBit.AddCondition<ConBoost>();
            }
        }
    }
}