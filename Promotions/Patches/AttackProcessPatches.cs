using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.Berserker;
using PromotionMod.Stats.Hermit;
using PromotionMod.Stats.Ranger;
using PromotionMod.Trait;
using PromotionMod.Trait.Machinist;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(AttackProcess))]
public class AttackProcessPatches
{
    [HarmonyPatch(nameof(AttackProcess.CalcHit))]
    [HarmonyPrefix]
    internal static bool CalcHitPatch(AttackProcess __instance, ref bool __result)
    {
        if (__instance.CC != null && __instance.CC.HasCondition<ConDeathbringer>())
        {
            __instance.crit = true;
            __result = true;
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(AttackProcess.Perform))]
    [HarmonyPrefix]
    internal static bool PerformPrefixPatch(AttackProcess __instance, int count, bool hasHit, ref float dmgMulti, ref bool maxRoll, bool subAttack)
    {
        if (__instance.TC.isChara && __instance.CC.Evalue(Constants.FeatHermit) > 0)
        {
            // Hermits - When the target is afflicted with Sleep/Paralyze/Faint Conditions, guarantees crits.
            if (__instance.TC.Chara.HasCondition<ConSleep>() || __instance.TC.Chara.HasCondition<ConParalyze>() || __instance.TC.Chara.HasCondition<ConFaint>())
            {
                maxRoll = true;
                dmgMulti += 0.1F;
            }
            
            // Hermits - If you have Shadow Shroud on, your attack has a 25% chance of revealing you, but your attacks will also do additional damage.
            ConShadowShroud shrouded = __instance.CC.GetCondition<ConShadowShroud>();
            if (shrouded != null)
            {
                if (EClass.rnd(4) == 0) shrouded.Kill();
                dmgMulti += .25F;
            }
        }
        return true;
    }
    
    [HarmonyPatch(nameof(AttackProcess.Perform))]
    [HarmonyPostfix]
    internal static void PerformPostfixPatch(AttackProcess __instance, int count, bool hasHit, ref float dmgMulti, ref bool maxRoll, bool subAttack)
    {
        // Machinist / General - Rocket Launcher
        if (__instance.TC.isChara && __instance.weapon.trait is TraitToolRocketLauncher && __instance.hit)
        {
            // Calculate the explosive power from the Rocket Power
            int power = EClass.curve((100 + __instance.weapon.ammoData._material.hardness * 10) * (100 + __instance.weapon.ammoData.encLV) / 100, 400, 100);
            
            // Proc an explosion at the location.
            ActEffect.ProcAt(EffectId.Explosive, power, BlessedState.Normal, __instance.CC, __instance.TC, __instance.TC.pos, isNeg: true, new ActRef
            {
                refThing = __instance.weapon.ammoData,
                aliasEle = "eleImpact"
            });
        }
        
        // Ranger - Gimmick Coatings. Does not work on Canes or Rocket Launchers.
        if (__instance.CC.HasCondition<ConGimmickCoating>() &&
            !subAttack &&
            __instance.TC.isChara &&
            __instance is { hit: true, IsRanged: true, toolRange: not null } &&
            __instance.weapon.trait is not TraitToolRangeCane && __instance.weapon.trait is not TraitToolRocketLauncher)
        {
            ConGimmickCoating coating = __instance.CC.GetCondition<ConGimmickCoating>();
            if (Enum.TryParse<Constants.RangerCoatings>(coating.GimmickType, out Constants.RangerCoatings coatingType))
            {
                switch (coatingType)
                {
                    case Constants.RangerCoatings.HammerCoating:
                        __instance.TC.Chara.AddCondition<ConFaint>(coating.power);
                        break;
                    case Constants.RangerCoatings.BladedCoating:
                        __instance.TC.Chara.AddCondition<ConBleed>(coating.power);
                        break;
                    case Constants.RangerCoatings.ParalyticCoating:
                        __instance.TC.Chara.AddCondition<ConParalyze>(coating.power);
                        break;
                    case Constants.RangerCoatings.PoisonCoating:
                        __instance.TC.Chara.AddCondition<ConPoison>(coating.power);
                        break;
                    case Constants.RangerCoatings.ShatterCoating:
                        // Try to split to two nearby targets
                        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(__instance.TC.pos, 2F, __instance.CC, false, true);
                        Chara target1 = targets.RandomItem();
                        targets.Remove(target1);
                        Chara target2 = targets.RandomItem();
                        if (target1 != null)
                        {
                            AttackProcess.Current.Prepare(Act.CC, __instance.weapon, target1, target1.pos);
                            AttackProcess.Current.Perform(count, hasHit, dmgMulti, maxRoll, true);
                        }
                        if (target2 != null)
                        {
                            AttackProcess.Current.Prepare(Act.CC, __instance.weapon, target2, target2.pos);
                            AttackProcess.Current.Perform(count, hasHit, dmgMulti, maxRoll, true);
                        }
                        break;
                }
            }
            else
            {
                // Invalid Condition somehow? Get rid of it.
                Msg.Say("Invalid Ranger Coating.");
                coating.Kill();
            }
        }
    }
}