using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
using PromotionMod.Stats.Berserker;
using PromotionMod.Stats.Hermit;
using PromotionMod.Stats.Machinist;
using PromotionMod.Stats.Ranger;
using PromotionMod.Stats.Sentinel;
using PromotionMod.Stats.Sharpshooter;
using PromotionMod.Stats.Sovereign;
using PromotionMod.Stats.Spellblade;
using UnityEngine;
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

        if (__instance.CC.Evalue(Constants.FeatHermit) > 0 &&
            __instance.TC != null &&
            (__instance.TC.isChara && __instance.TC.Chara.HasCondition<ConSleep>() || __instance.TC.Chara.HasCondition<ConParalyze>() || __instance.TC.Chara.HasCondition<ConFaint>()))
        {
            __instance.crit = true;
            __instance.dMulti += 0.1F;
            __result = true;
            return false;
        }

        return true;
    }

    [HarmonyPatch(nameof(AttackProcess.Perform))]
    [HarmonyPrefix]
    internal static bool PerformPrefixPatch(AttackProcess __instance, int count, bool hasHit, ref float dmgMulti, ref bool maxRoll, bool subAttack)
    {
        if (Act.TC != null && Act.TC.isChara && Act.CC != null && Act.CC.Evalue(Constants.FeatHermit) > 0)
        {
            // Hermits - When the target is afflicted with Sleep/Paralyze/Faint Conditions, guarantees crits.
            if (Act.TC.Chara.HasCondition<ConSleep>() || Act.TC.Chara.HasCondition<ConParalyze>() || Act.TC.Chara.HasCondition<ConFaint>())
            {
                maxRoll = true;
                dmgMulti += 0.1F;
            }

            // Hermits - If you have Shadow Shroud on, your attack has a 25% chance of revealing you, but your attacks will also do additional damage.
            ConShadowShroud shrouded = Act.CC.GetCondition<ConShadowShroud>();
            if (shrouded != null)
            {
                if (EClass.rnd(4) == 0) shrouded.Kill();
                dmgMulti += .25F;
            }
        }

        // Sharpshooter - Charged Chamber applies the mana consumed (power) as a damage multiplier. Caps at 5x damage.
        // BALANCE: This might... be a little too strong later?
        if (Act.CC != null && Act.CC.Chara.Evalue(Constants.FeatSharpshooter) > 0 && Act.CC.HasCondition<ConChargedChamber>() && __instance.IsRanged)
        {
            ConChargedChamber charge = Act.CC.GetCondition<ConChargedChamber>();
            dmgMulti += Math.Max(charge.power / 100F, 5F);
        }

        return true;
    }

    [HarmonyPatch(nameof(AttackProcess.Perform))]
    [HarmonyPostfix]
    internal static void PerformPostfixPatch(AttackProcess __instance, int count, bool hasHit, ref float dmgMulti, ref bool maxRoll, bool subAttack)
    {
        Card target = __instance.TC;
        Chara originChara = __instance.CC;
        
        ILookup<Type, Condition> originConditions = originChara.conditions.ToLookup(c => c.GetType());
        
        // Berserker - Lifebreak - On hit, inflict additional damage based on missing HP of the user.
        if (originConditions.Contains(typeof(ConLifebreakAttack)) &&
            !subAttack &&
            __instance is { IsRanged: false, isThrow: false })
        {
            if (__instance.TC.isChara && __instance.hit)
            {
                int damage = originChara.Chara.MaxHP - originChara.Chara.hp;
                damage = HelperFunctions.SafeMultiplier(damage, 1.3F);
                target.Chara.DamageHP(damage, AttackSource.Melee, originChara);   
            }
            originConditions[typeof(ConLifebreakAttack)].Single().Kill();
        }
        
        // Machinist - Heavyarms mode followup rockets.
        if (originConditions.Contains(typeof(StanceHeavyarms)) &&
            !subAttack &&
            __instance.TC.isChara &&
            __instance is { IsRanged: true, toolRange: not null })
        {
            Condition heavyArms = originConditions[typeof(StanceHeavyarms)].Single();
            // TODO: Text
            __instance.CC.Say("machinist_heavyarms_followup".langGame(), Act.CC);
            __instance.CC.PlaySound("missile");
            ActEffect.ProcAt(EffectId.Rocket, heavyArms.power, BlessedState.Normal, __instance.CC, null, __instance.TC.pos, true, new ActRef
            {
                origin = __instance.CC.Chara,
                aliasEle = "eleImpact"
            });
        }

        // Ranger - Gimmick Coatings. Does not work on Canes
        if (originConditions.Contains(typeof(ConGimmickCoating)) &&
            !subAttack &&
              __instance is { IsRanged: true, toolRange: not null } &&
              __instance.weapon.trait is not TraitToolRangeCane)
        {
            ConGimmickCoating coating = (ConGimmickCoating)originConditions[typeof(ConGimmickCoating)].Single();
            if (__instance.hit && __instance.TC.isChara)
            {
                if (Enum.TryParse(coating.GimmickType, out Constants.RangerCoatings coatingType))
                {
                    switch (coatingType)
                    {
                        case Constants.RangerCoatings.HammerCoating:
                            ActEffect.ProcAt(EffectId.Debuff, coating.power, BlessedState.Normal, Act.CC, __instance.TC.Chara, __instance.TC.Chara.pos, true, new ActRef
                            {
                                origin = Act.CC.Chara,
                                n1 = nameof(ConFaint)
                            });
                            break;
                        case Constants.RangerCoatings.BladedCoating:
                            ActEffect.ProcAt(EffectId.Debuff, coating.power, BlessedState.Normal, Act.CC, __instance.TC.Chara, __instance.TC.Chara.pos, true, new ActRef
                            {
                                origin = Act.CC.Chara,
                                n1 = nameof(ConBleed)
                            });
                            break;
                        case Constants.RangerCoatings.ParalyticCoating:
                            ActEffect.ProcAt(EffectId.Debuff, coating.power, BlessedState.Normal, Act.CC, __instance.TC.Chara, __instance.TC.Chara.pos, true, new ActRef
                            {
                                origin = Act.CC.Chara,
                                n1 = nameof(ConParalyze)
                            });
                            break;
                        case Constants.RangerCoatings.PoisonCoating:
                            ActEffect.ProcAt(EffectId.Debuff, coating.power, BlessedState.Normal, Act.CC, __instance.TC.Chara, __instance.TC.Chara.pos, true, new ActRef
                            {
                                origin = Act.CC.Chara,
                                n1 = nameof(ConPoison)
                            });
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
            coating.Mod(-1);
        }
        
        // Sentinel - Shield Bash
        if (originConditions.Contains(typeof(ConShieldSmiteAttack)) &&
            !subAttack &&
            __instance is { IsRanged: false, isThrow: false })
        {
            ConShieldSmiteAttack smite = (ConShieldSmiteAttack)originConditions[typeof(ConShieldSmiteAttack)].Single();
            if (hasHit && __instance.TC.isChara)
            {
                int shieldSkill = originChara.Evalue(123);
                int basherEnc = originChara.Evalue(381);
                long shieldPower = FeatSentinel.GetShieldPower(originChara);
                shieldPower += (int)(target.MaxHP * .125F);
                target.DamageHP(shieldPower, AttackSource.Melee, originChara);
                if (target.IsAliveInCurrentZone)
                {
                    if (EClass.rnd(2) == 0)
                        target.Chara.AddCondition<ConDim>(50 + (int)Mathf.Sqrt(shieldSkill) * 10);
                    target.Chara.AddCondition<ConParalyze>(EClass.rnd(2), true);
                }
                AttackProcess.ProcShieldEncs(originChara, target, 500 + basherEnc);
            }
            smite.Kill();
        }

        // Sharpshooter - Ranged Attacks will automatically apply the Suppress effect, regardless of hitting or not
        if (__instance.TC is { isChara: true } && originChara.Evalue(Constants.FeatSharpshooter) > 0 && __instance.IsRanged)
        {
            target.Chara.AddCondition<ConSupress>();
        }

        // Sovereign - Order Sword allows follow-up attacks from allies in Coherency.
        if (originConditions.Contains(typeof(ConOrderSword)) && __instance.TC is { isChara: true })
        {
            ConOrderSword orderSword = (ConOrderSword)originConditions[typeof(ConOrderSword)].Single();
            if (orderSword is { FollowUpAvailable: true })
            {
                // Neighboring allies with OrderSword will also try to followup attack.
                bool followUpPerformed = false;
                Act.CC.pos.ListCharasInNeighbor(delegate(Chara c)
                {
                    if (c == Act.CC || c.IsHostile(Act.CC) || !c.HasCondition<ConOrderSword>())
                    {
                        return false;
                    }

                    // Melee / Ranged / Throw attack.
                    if (ACT.Melee.CanPerform(c, __instance.TC, __instance.TC.pos))
                    {
                        // TODO Text: Follow up text.
                        new ActMelee().Perform(c, __instance.TC, __instance.TC.pos);
                        followUpPerformed = true;
                        return true;
                    }
                    if (ACT.Ranged.CanPerform(c, __instance.TC, __instance.TC.pos))
                    {
                        new ActRanged().Perform(c, __instance.TC, __instance.TC.pos);
                        followUpPerformed = true;
                        return true;
                    }
                    if (ACT.Throw.CanPerform(c, __instance.TC, __instance.TC.pos))
                    {
                        new ActThrow().Perform(c, __instance.TC, __instance.TC.pos);
                        followUpPerformed = true;
                        return true;
                    }

                    return false;
                });

                orderSword.FollowUpAvailable = !followUpPerformed;
            }
        }
        
        // Spellblade - Crushing Strike will attack a random body part.
        // Get the body parts of the target.
        if (originConditions.Contains(typeof(ConCrushingStrikeAttack)) &&
            !subAttack &&
            __instance is { IsRanged: false, isThrow: false })
        {
            if (__instance.hit && __instance.TC.isChara)
            {
                int power = originConditions[typeof(ConCrushingStrikeAttack)].Single().power;
                BodySlot partTarget = target.Chara.body.slots.RandomItem();
                int breakAmount = (int)HelperFunctions.SigmoidScaling(power, 10, 25);
                
                // Depending on the Body Part, attempt to inflict different condition(s).
                // TODO Text: Actually add this.
                originChara.Say("spellblade_crushing_strike".lang(originChara.NameSimple, target.Chara.NameSimple, partTarget.name));
                switch (partTarget.elementId)
                {
                    case 30: // Head
                        ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, originChara, target, target.pos, true, new ActRef
                        {
                            origin = originChara,
                            n1 = nameof(ConBlind)
                        });
                        ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, originChara, target, target.pos, true, new ActRef
                        {
                            origin = originChara,
                            n1 = nameof(ConDim)
                        });
                        break;
                    case 31: // Neck
                        ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, originChara, target, target.pos, true, new ActRef
                        {
                            origin = originChara,
                            n1 = nameof(ConSilence)
                        });
                        ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, originChara, target, target.pos, true, new ActRef
                        {
                            origin = originChara,
                            n1 = nameof(ConSuffocation)
                        });
                        break;
                    case 32: // Torso
                    case 33: // Back
                    case 37: // Waist
                        target.Chara.AddCondition(SubPoweredCondition.Create(nameof(ConArmorBreak), power, breakAmount));
                        break;
                    case 34: // Arm
                    case 35: // Hand
                    case 36: // Finger
                        ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, originChara, target, target.pos, true, new ActRef
                        {
                            origin = originChara,
                            n1 = nameof(ConDisable)
                        });
                        break;
                    case 38: // Leg
                    case 39: // Foot
                        target.Chara.AddCondition(SubPoweredCondition.Create(nameof(ConSpeedBreak), power, breakAmount));
                        break;
                }
            }
            
            originConditions[typeof(ConCrushingStrikeAttack)].Single().Kill();
        }
    }
}