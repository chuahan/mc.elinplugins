using System;
using System.Linq;
using BepInEx;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities.Elementalist;
using PromotionMod.Stats;
using PromotionMod.Stats.AdvCombatSkills;
using PromotionMod.Stats.Jenei;
using PromotionMod.Stats.WitchHunter;
using UnityEngine;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActEffect))]
public class ActEffectPatches
{
    [HarmonyPatch(nameof(ActEffect.ProcAt))]
    [HarmonyPrefix]
    internal static bool ProcAtPrefix(ActEffect __instance, EffectId id, ref int power, BlessedState state, ref Card cc, ref Card tc, Point tp, bool isNeg, ActRef actRef)
    {
        if (cc is { isChara: true })
        {
            Chara chara = cc.Chara;
            // Bit Overrides
            if (id is EffectId.Funnel)
            {
                if (ActEffectPatches.HasAltSummonBits(chara))
                {
                    ActEffectPatches.SummonAltBits(chara, power, tp, actRef);
                    return false;
                }
            }

            // Hexer has a 1/10 to guarantee Hex application.
            // Hexer also will double hex power if they themselves have a debuff.
            if (cc.HasElement(Constants.FeatHexer) && id is EffectId.Debuff)
            {
                if (tc is { isChara: true })
                {
                    if (cc.Chara.conditions.Any(x => x.Type == ConditionType.Debuff || x.Type == ConditionType.Bad))
                    {
                        power = (int)(power * 1.5F);
                    }
                    Chara targetChara = tc.Chara;
                    chara.DoHostileAction(tc);
                    bool isPowerful = tc.IsPowerful;
                    string conditionAlias = actRef.n1;
                    if (conditionAlias == "ConSuffocation") power = power * 2 / 3;

                    int debuffResist = tc.WIL * (isPowerful ? 20 : 5);
                    ConHolyVeil holyVeil = targetChara.GetCondition<ConHolyVeil>();
                    if (holyVeil != null) debuffResist += holyVeil.power * 5;

                    if (EClass.rnd(power) < debuffResist / EClass.sources.stats.alias[conditionAlias].hexPower && EClass.rnd(10) != 0)
                    {
                        if (EClass.rnd(10) != 0) // This is a somewhat stripped down version of the original Debuff with this extra if statement.
                        {
                            tc.Say("debuff_resist", tc);
                            return false;
                        }
                    }
                    targetChara.AddCondition(Condition.Create(conditionAlias, power, delegate(Condition con)
                    {
                        con.givenByPcParty = chara.IsPCParty;
                        (con as ConDeathSentense)?.SetChara(chara);
                        if (!actRef.aliasEle.IsEmpty())
                        {
                            con.SetElement(EClass.sources.elements.alias[actRef.aliasEle].id);
                        }
                    }));
                    if (EClass.core.config.game.waitOnDebuff && !chara.IsPC)
                    {
                        EClass.Wait(0.3f, tc);
                    }
                }
            }

            // Elementalist - Track Spellcasts
            if (chara.HasElement(Constants.FeatElementalist) &&
                actRef.aliasEle != null &&
                !actRef.aliasEle.IsEmpty() &&
                actRef.act is not ActElementalFury &&
                actRef.act is not ActElementalExtinction)
            {
                if (Constants.ElementAliasLookup.ContainsValue(actRef.aliasEle))
                {
                    int element = Constants.ElementIdLookup[actRef.aliasEle];
                    ConElementalist? elementalist = chara.GetCondition<ConElementalist>() ?? chara.AddCondition<ConElementalist>() as ConElementalist;
                    if (tc is { isChara: true })
                    {
                        elementalist?.AddElementalOrb(element, tc.Chara.IsHostile(chara) ? tc.Chara : null);
                    }
                }
            }

            // Jenei - Track Spellcasts for Impact/Fire/Cold/Lightning
            if (chara.HasElement(Constants.FeatJenei) && actRef.aliasEle != null && !actRef.aliasEle.IsEmpty())
            {
                if (Constants.ElementAliasLookup.ContainsValue(actRef.aliasEle))
                {
                    int element = Constants.ElementIdLookup[actRef.aliasEle];
                    if (element is Constants.EleImpact or Constants.EleFire or Constants.EleCold or Constants.EleLightning)
                    {
                        ConJenei? jenei = chara.GetCondition<ConJenei>() ?? chara.AddCondition<ConJenei>() as ConJenei;
                        jenei?.AddElement(element);
                    }
                }
            }

            if (tc is { isChara: true })
            {
                // Witch Hunter - Magic Reflect will reflect targeted magic.
                if (tc.Chara.HasCondition<ConMagicReflect>())
                {
                    ConMagicReflect magicReflect = tc.Chara.GetCondition<ConMagicReflect>();
                    magicReflect.Mod(-1);
                    if (magicReflect.value <= 0) magicReflect.Kill();

                    // Swap the TC and CC
                    (tc, cc) = (cc, tc);
                }

                // Witch Hunter - Target gets silenced with Bane in addition to excommunication.
                if (cc.Chara.HasElement(Constants.FeatWitchHunter) && string.Equals(actRef.n1, "ConBane", StringComparison.InvariantCultureIgnoreCase))
                {
                    tc.Chara.AddCondition<ConSilence>(power);
                }

                // Combat Skill Activations for Spells.
                if (cc.HasElement(Constants.FeatLunaId))
                {
                    // Luna - 25%
                    if (EClass.rnd(4) == 0)
                    {
                        cc.Say("luna_activation".langGame(Act.CC.NameSimple));
                        cc.PlaySound("warcry");
                        // Nihil can deny this ability
                        if (tc.HasElement(Constants.FeatNihilId))
                        {
                            tc.Say("nihil_activation".langGame(Act.TC.NameSimple));
                            tc.PlaySound("shield_bash");
                        }
                        else
                        {
                            // Luna is applied to the enemy to pierce their resistances.
                            tc.Chara.AddCondition<ConLuna>(force: true);
                        }
                    }
                }
                else if (Act.CC.HasElement(Constants.FeatLunaPlusId))
                {
                    // Luna+ - 100% Chance
                    Act.CC.Say("luna_activation".langGame(Act.CC.NameSimple));
                    Act.CC.PlaySound("warcry");
                    if (Act.TC.HasElement(Constants.FeatNihilId))
                    {
                        Act.TC.Say("nihil_activation".langGame(Act.TC.NameSimple));
                        Act.TC.PlaySound("shield_bash");
                    }
                    else
                    {
                        // Luna is applied to the enemy to pierce their resistances.
                        Act.TC.Chara.AddCondition<ConLuna>(force: true);
                    }
                }
                else if (Act.CC.HasElement(Constants.FeatSolId))
                {
                    // Sol - 30%
                    if (EClass.rnd(3) == 0)
                    {
                        Act.CC.Say("sol_activation".langGame(Act.CC.NameSimple));
                        Act.CC.PlaySound("warcry");
                        if (Act.TC.HasElement(Constants.FeatNihilId))
                        {
                            Act.TC.Say("nihil_activation".langGame(Act.TC.NameSimple));
                            Act.TC.PlaySound("shield_bash");
                        }
                        else
                        {
                            Act.CC.AddCondition<ConSol>();
                        }
                    }
                }
                else if (Act.CC.HasElement(Constants.FeatRendHeavenId))
                {
                    // Rend Heaven - 50%
                    string parentAttribute = EClass.sources.elements.map[actRef.act.id].aliasParent;
                    if (!parentAttribute.IsNullOrWhiteSpace())
                    {
                        if (EClass.rnd(2) == 0)
                        {
                            Act.CC.Say("sol_activation".langGame(Act.CC.NameSimple));
                            Act.CC.PlaySound("warcry");
                            if (Act.TC.HasElement(Constants.FeatNihilId))
                            {
                                Act.TC.Say("nihil_activation".langGame(Act.TC.NameSimple));
                                Act.TC.PlaySound("shield_bash");
                            }
                            else
                            {
                                // For Spells, using the element, pull the element out and use that to get the parent attribute.
                                // Then basically do their "GetPower" call and halve it to add to the power.
                                if (actRef.act != null)
                                {
                                    Msg.Nerun($"Starting Power: {power}");
                                    power += actRef.act.GetPower(Act.TC) / 2;
                                    // If Rend Heaven was activated, snapshot the opponents stats, average it and apply it as power.
                                    int attributesBorrowed = (Act.TC.Evalue(70) +
                                                              Act.TC.Evalue(71) +
                                                              Act.TC.Evalue(72) +
                                                              Act.TC.Evalue(73) +
                                                              Act.TC.Evalue(74) +
                                                              Act.TC.Evalue(75) +
                                                              Act.TC.Evalue(76) +
                                                              Act.TC.Evalue(77)) /
                                                             8;
                                    Act.CC.AddCondition<ConRendHeaven>(attributesBorrowed);
                                    Msg.Nerun($"Stole Power: {power}");
                                }
                            }
                        }
                    }
                }
            }
        }
        return true;
    }

    private static bool HasAltSummonBits(Chara caster)
    {
        return caster.GetFlagValue(Constants.PromotionFeatFlag) is Constants.FeatElementalist or Constants.FeatBattlemage or Constants.FeatSpellblade;
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
                int bitCount = Math.Max(2, Mathf.Clamp(power / 100, 1, 5) + (power >= 100 ? EClass.rnd(2) : 0));
                ActEffectPatches.SummonBitInternal(Constants.NormalBitCharaId, caster, power, tp, element, bitCount, true);
                // TODO: Apply Brightness of Life?
                break;
            // Battlemages Summon one Shield Bit and one normal bit.
            case Constants.FeatBattlemage:
                ActEffectPatches.SummonBitInternal(Constants.ShieldBitCharaId, caster, power, tp, element, 1, false);
                ActEffectPatches.SummonBitInternal(Constants.NormalBitCharaId, caster, power, tp, element, 1, false);
                break;
            // Spellblades Summon Sword Bits
            case Constants.FeatSpellblade:
                ActEffectPatches.SummonBitInternal(Constants.SwordBitCharaId, caster, power, tp, element, 2, false);
                break;
            default:
                return;
        }
    }

    private static void SummonBitInternal(string type, Chara caster, int power, Point tp, Element element, int count, bool buff)
    {
        int levelOverride = power / 15;
        if (caster.IsPCFaction) levelOverride = Math.Max(EClass.player.stats.deepest, levelOverride);

        for (int i = 0; i < count; i++)
        {
            Chara summonedBit = CharaGen.Create(type);
            summonedBit.SetMainElement(element.source.alias, element.Value, true);
            summonedBit.SetSummon(30 + power / 10);
            summonedBit.SetLv(levelOverride);
            summonedBit.interest = 0;
            EClass._zone.AddCard(summonedBit, tp.GetNearestPoint(false, false));
            summonedBit.PlayEffect("teleport");
            summonedBit.MakeMinion(caster);
            if (buff)
            {
                summonedBit.AddCondition<ConProtection>(power);
                summonedBit.AddCondition<ConBoost>();
            }
        }
    }
}