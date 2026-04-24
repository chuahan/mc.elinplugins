using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities.HolyKnight;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
using PromotionMod.Stats.AdvCombatSkills;
using PromotionMod.Stats.Artificer;
using PromotionMod.Stats.Battlemage;
using PromotionMod.Stats.Gambler;
using PromotionMod.Stats.Harbinger;
using PromotionMod.Stats.Headhunter;
using PromotionMod.Stats.Hermit;
using PromotionMod.Stats.HolyKnight;
using PromotionMod.Stats.Justicar;
using PromotionMod.Stats.Runeknight;
using PromotionMod.Stats.Sniper;
using PromotionMod.Stats.Sovereign;
using PromotionMod.Stats.Spellblade;
using PromotionMod.Stats.WarCleric;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Card))]
public class CardDamageHPPatches
{
    private static readonly List<AttackSource> MagicAttackSources = new List<AttackSource>
    {
        AttackSource.MagicArrow,
        AttackSource.MagicHand,
        AttackSource.MagicSword,
        AttackSource.MoonSpear,
        AttackSource.None
    };

    [HarmonyPatch(nameof(Card.DamageHP), typeof(long), typeof(int), typeof(int), typeof(AttackSource), typeof(Card), typeof(bool), typeof(Thing), typeof(Chara))]
    [HarmonyPrefix]
    internal static bool OnDamageHP_Patches(Card __instance, ref long dmg, ref int ele, ref int eleP, AttackSource attackSource, Card? origin, bool showEffect, Thing weapon,
        Chara originalTarget)
    {
        //if (__instance.isChara && ActiveAttackSources.Contains(attackSource))
        // This is the same attack source conditions as Wall of Flesh, so active attack sources.
        if (__instance.isChara && (uint)(attackSource - 3) > 2u && (uint)(attackSource - 13) > 4u)
        {
            Chara target = __instance.Chara;
            Chara? originChara = origin?.Chara;

            // Target Conditionals
            ILookup<Type, Condition> targetConditions = target.conditions.ToLookup(c => c.GetType());

            // Origin Conditionals
            if (originChara != null)
            {
                ILookup<Type, Condition>? originConditions = originChara.conditions.ToLookup(c => c.GetType());

                // Metal Breaker - If target has Metal, reduce it to 0.
                if (originChara.HasElement(Constants.FeatMetalBreakerId) && target.HasElement(1218) && !targetConditions.Contains(typeof(ConMetalBreak)))
                {
                    target.Say("metalbreaker_activation".langGame(target.NameSimple));
                    target.PlaySound("parry");
                    if (!HelperFunctions.NihilActivated(originChara))
                    {
                        target.AddCondition<ConMetalBreak>(target.Evalue(1218), true);
                    }
                }
                else if (originChara.HasElement(Constants.FeatVengeanceId))
                {
                    // Vengeance - Adds user's missing HP as base damage pre-multipliers.
                    target.Say("vengeance_activation".langGame(target.NameSimple));
                    target.PlaySound("parry");
                    if (!HelperFunctions.NihilActivated(originChara))
                    {
                        dmg += originChara.MaxHP - originChara.hp;
                    }
                }
                else if (originChara.HasElement(Constants.FeatAstraId))
                {
                    // Astra - Does 250% damage. Hard to get multi attacks going so this will have to do.
                    target.Say("astra_activation".langGame(target.NameSimple));
                    target.PlaySound("parry");
                    if (!HelperFunctions.NihilActivated(originChara))
                    {
                        dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 2.5F);
                    }
                }

                // Artificer - If the target is riding a Titan Golem, divert damage to the Titan Golem instead.
                if (target.ride is { id: Constants.TitanGolemCharaId })
                {
                    target.ride.DamageHP(dmg, ele, eleP, attackSource, origin, showEffect, weapon, target);
                    return false;
                }

                // Gambler - Feeling Lucky will change the damage dealt or taken. -10 to +10.
                if (originConditions.Contains(typeof(StanceFeelingLucky)))
                {
                    Dice luckyDice = new Dice { num = 1, sides = 20, card = originChara };
                    int diceRoll = luckyDice.Roll() - 9;
                    float damageMulti = (10 + diceRoll) / 10F;
                    dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, damageMulti);
                }

                if (targetConditions.Contains(typeof(StanceFeelingLucky)))
                {
                    Dice luckyDice = new Dice { num = 1, sides = 20, card = target };
                    int diceRoll = luckyDice.Roll() - 9;
                    float damageMulti = (10 - diceRoll) / 10F;
                    dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, damageMulti);
                }
                
                // Hermits - Opportunist - 10% damage increase if the target is afflicted with specific debuffs. Spells can't crit though.
                if (targetConditions.Contains(typeof(ConParalyze)) ||
                    targetConditions.Contains(typeof(ConBleed)) ||
                    targetConditions.Contains(typeof(ConSleep)) ||
                    targetConditions.Contains(typeof(ConFaint)) ||
                    targetConditions.Contains(typeof(ConPoison)))
                {
                    dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 1.1F);
                }

                // Hermits - If you have Shadow Shroud on, your attacks has a 25% chance of revealing you, but you also gain a 25% damage increase.
                if (originConditions.Contains(typeof(ConShadowShroud)))
                {
                    dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 1.25F);
                    if (EClass.rnd(4) == 0)
                    {
                        originConditions[typeof(ConShadowShroud)].Single().Kill();
                        originChara.AddCooldown(Constants.ActShadowShroudId, 5);
                    }
                }

                // Hexer - When applying spell damage, there is a 10% chance to apply a hex out of a pool.
                if (originChara.MatchesPromotion(Constants.FeatHexer) && MagicAttackSources.Contains(attackSource))
                {
                    if (EClass.rnd(10) == 0)
                    {
                        Msg.Say("hexer_hextouch".langGame(originChara.NameSimple));
                        FeatHexer.ApplyCondition(__instance.Chara, originChara, 100, true);
                    }
                }

                // Hexer - When taking damage, there is a 10% chance to force-apply a hex out of a pool in retaliation.
                if (target.MatchesPromotion(Constants.FeatHexer))
                {
                    if (EClass.rnd(10) == 0)
                    {
                        Msg.Say("hexer_revenge".langGame(originChara.NameSimple));
                        FeatHexer.ApplyCondition(originChara, __instance.Chara, 100, true);
                    }
                }

                // Harbinger - Every active Miasma on the target boosts damage from Harbingers by 5%.
                if (originChara.MatchesPromotion(Constants.FeatHarbinger) && targetConditions.Contains(typeof(ConHarbingerMiasma)))
                {
                    int miasmaCount = target.conditions.Count(con => con is ConHarbingerMiasma);
                    dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 1 + miasmaCount * 0.05F);
                }

                // Headhunter - Damage is increased by 25% against higher quality enemies.
                // Headhunter - Damage is increased by 3% per Headhunter stack.
                if (originChara.HasElement(Constants.FeatHeadhunter))
                {
                    if (target.source.quality >= 3)
                    {
                        dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 1.25F);
                    }

                    if (originConditions.Contains(typeof(ConHeadhunter)))
                    {
                        ConHeadhunter headhunter = (ConHeadhunter)originConditions[typeof(ConHeadhunter)].Single();
                        dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 1 + headhunter.GetStacks() * .03F);
                    }
                }

                // Headhunter - Damage taken is decreased by 3% per Headhunter stack.
                if (target.HasElement(Constants.FeatHeadhunter))
                {
                    if (targetConditions.Contains(typeof(ConHeadhunter)))
                    {
                        ConHeadhunter headhunter = (ConHeadhunter)targetConditions[typeof(ConHeadhunter)].Single();
                        dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 1 - headhunter.GetStacks() * .03F);
                    }
                }

                // Saint - If the Saint and the target share the same religion, the Saint can attempt to convert the opponent.
                if (originChara.MatchesPromotion(Constants.FeatSaint) && originChara.faith.id == target.faith.id)
                {
                    // Saint's faith must be higher than the target.
                    // Must be a convertable target.
                    // Succeed a 1/10 roll.
                    // Random WIL + FAITH + 10 less than the Saint's Faith.
                    if (!target.IsMinion &&
                        target.CanBeTempAlly(originChara) &&
                        originChara.Evalue(SKILL.faith) > target.Evalue(SKILL.faith) &&
                        EClass.rnd(10) == 0 &&
                        EClass.rnd(target.WIL + target.Evalue(SKILL.faith) + 10) < originChara.Evalue(SKILL.faith))
                    {
                        Msg.Say("saint_convert_target".langGame(originChara.NameSimple, target.NameSimple));
                        target.PlayEffect("aura_heaven");
                        target.ShowEmo(Emo.love);
                        target.lastEmo = Emo.angry;
                        target.Chara.MakeMinion(originChara.IsPCParty ? EClass.pc : originChara);
                        return false;
                    }
                }

                // Sniper - Targets specific body parts.
                if (originConditions.Contains(typeof(ConSniperTarget)))
                {
                    ConSniperTarget sniperTarget = (ConSniperTarget)originConditions[typeof(ConSniperTarget)].Single();
                    switch (sniperTarget.Target)
                    {
                        case ConSniperTarget.TargetPart.Hand:
                            target.AddCondition<ConDisable>(sniperTarget.power);
                            break;
                        case ConSniperTarget.TargetPart.Head:
                            // 25% chance to cull the target at or under 30% HP.
                            if (target.hp <= target.MaxHP * 0.30F && EClass.rnd(4) == 0)
                            {
                                target.DamageHP(target.MaxHP, AttackSource.Finish, origin);
                            }
                            target.AddCondition<ConDim>(sniperTarget.power);
                            target.AddCondition<ConSilence>(sniperTarget.power);
                            break;
                        case ConSniperTarget.TargetPart.Legs:
                            int breakAmount = (int)HelperFunctions.SigmoidScaling(sniperTarget.power, 10, 25);
                            target.AddCondition(SubPoweredCondition.Create(nameof(ConSpeedBreak), sniperTarget.power, breakAmount));
                            break;
                    }
                }

                // Sovereign - Chaos Stance Increases damage by 10%.
                if (originConditions.Contains(typeof(ConSovereignChaos)))
                {
                    dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 1.1F);
                }


                // Spellblade - If the Spellblade is using Siphoning Blade. Do no damage and instead deal the 50% damage as MP instead, absorbing it.
                if (originChara.MatchesPromotion(Constants.FeatSpellblade))
                {
                    if (originConditions.Contains(typeof(ConSiphoningBlade)))
                    {
                        dmg = (int)(dmg * 0.5F);
                        target.mana.Mod((int)(0 - dmg));
                        originChara.mana.Mod((int)dmg);
                        return false;
                    }
                }

                // War Cleric / Holy Knight - Shining Blade causes the healer to absorb 30% of the damage dealt.
                if (originConditions.Contains(typeof(ConShiningBlade)))
                {
                    int heal = (int)(dmg * 0.3F);
                    originChara.HealHP(heal, HealSource.Magic);
                    ConShiningBlade shiningBlade = (ConShiningBlade)originConditions[typeof(ConShiningBlade)].Single();
                    shiningBlade.Mod(-1);
                }

                // Spellblade and Elementalist - Excel at applying status effects. eleP doubled.
                if (originChara.MatchesPromotion(Constants.FeatSpellblade) ||
                    originChara.MatchesPromotion(Constants.FeatElementalist))
                {
                    eleP = HelperFunctions.SafeMultiplier(eleP, 2);
                }
            }

            // Artificer - Heavenly Pearl will decrease a damage instance by 25%
            if (targetConditions.Contains(typeof(ConHeavenlyEmbrace)))
            {
                ConHeavenlyEmbrace heavenlyEmbrace = (ConHeavenlyEmbrace)targetConditions[typeof(ConHeavenlyEmbrace)].Single();
                dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 0.75F);
                heavenlyEmbrace.Mod(-1);
            }

            // Holy Knight - Vanguard Stance: Redirect damage from allies to the Holy Knight in Vanguard Stance.
            // Does not redirect if the target is already the Holy Knight.
            if (!targetConditions.Contains(typeof(StanceVanguard)))
            {
                List<Chara> targetAllies = HelperFunctions.GetCharasWithinRadius(target.pos, 5f, target, true, false);
                if (targetAllies.Count(c => c.conditions.Any(cond => cond.GetType() == typeof(StanceVanguard))) > 0)
                {
                    Chara holyKnightAlly = targetAllies.First(c => c.conditions.Any(cond => cond.GetType() == typeof(StanceVanguard)));
                    holyKnightAlly.DamageHP(dmg, ele, eleP, attackSource, origin, showEffect, weapon, target);
                    return false;
                }
            }

            // Holy Knight - When taking damage, if currently in Deflection, reduce damage to zero and add a stack of Heavenly Host.
            if (targetConditions.Contains(typeof(ConDeflection)))
            {
                dmg = 0;
                target.AddCooldown(Constants.ActDeflectionId, -5);
                ConDeflection deflection = (ConDeflection)targetConditions[typeof(ConDeflection)].Single();
                ConHeavenlyHost? heavenlyHost = (ConHeavenlyHost)targetConditions[typeof(ConHeavenlyHost)].Single() ?? target.AddCondition<ConHeavenlyHost>() as ConHeavenlyHost;
                heavenlyHost?.AddStacks(1);
                ActSpearhead.SpawnHolySwordBit(deflection.power, target, target.pos);
            }

            // Holy Knight - Reduces damage based on Heavenly Host Condition Stacks (2% per stack, caps at 10)
            if (targetConditions.Contains(typeof(ConHeavenlyHost)))
            {
                ConHeavenlyHost heavenlyHost = (ConHeavenlyHost)targetConditions[typeof(ConHeavenlyHost)].Single();
                float damageMulti = 1 - heavenlyHost.GetStacks() * 0.02F;
                dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, damageMulti);

            }

            // Necromancer - Bone Armor. Reduces damage based on how many Skeleton Minions you have.
            // Caps at 75% damage reduction
            if (target.MatchesPromotion(Constants.FeatNecromancer))
            {
                int boneArmyCount = __instance.Chara.currentZone.ListMinions(__instance.Chara).Count(c => c.HasTag(CTAG.undead));
                float damageMulti = Math.Min(0.75F, boneArmyCount * 0.025F);
                dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, damageMulti);
            }

            // Rune Knight - Elemental Attunement. If damage received matches your attuned element, all damage is absorbed and added as stockpiled damage.
            if (targetConditions.Contains(typeof(ConElementalAttunement)))
            {
                ConElementalAttunement attunement = (ConElementalAttunement)targetConditions[typeof(ConElementalAttunement)].Single();
                if (attunement != null)
                {
                    attunement.StoredDamage += dmg;
                    dmg = 0;
                    return false;
                }
            }

            // Rune Knight - Runic Guard is removed and Elemental Attunement is added. All damage is absorbed.
            if (targetConditions.Contains(typeof(ConRunicGuard)) && ele != Constants.EleVoid)
            {
                target.RemoveCondition<ConRunicGuard>();
                ConElementalAttunement attunement = (ConElementalAttunement)target.AddCondition<ConElementalAttunement>();
                if (attunement != null)
                {
                    attunement.AttunedElement = ele;
                }

                // Absorbing up to 20% of the damage as mana and stamina.
                long restorationAmount = Math.Min(dmg / 5, 20);
                target.mana.Mod((int)restorationAmount);
                target.stamina.Mod((int)restorationAmount);
                return false;
            }

            // Rune Knight - Warding runes will reduce incoming damage by 20% in exchange for a charge.
            if (targetConditions.Contains(typeof(ConWardingRune)))
            {
                ((ConWardingRune)targetConditions[typeof(ConWardingRune)].Single()).Mod(-1);
                dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 0.8F);
            }

            // Sovereign - Law Stance Reduces damage by 10%.
            if (targetConditions.Contains(typeof(ConSovereignLaw)))
            {
                dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 0.9F);
            }

            // Sovereign - Barricade Order: Reduces damage based on # of allies neighboring you
            // 5% damage reduced per ally in coherence.
            if (targetConditions.Contains(typeof(ConOrderBarricade)))
            {
                float barricadeCoherence = 1 - target.pos.ListCharasInNeighbor(c => c == Act.CC || c.IsHostile(Act.CC)).Count * 0.05F;
                dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, barricadeCoherence);

            }

            // War Cleric - Sanctuary reduces all damage dealt by 75%.
            if (targetConditions.Contains(typeof(ConSanctuary)))
            {
                dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 0.25F);
            }

            // Afterimage - Fully negates one instance of damage.
            if (targetConditions.Contains(typeof(ConAfterimage)) && dmg != 0)
            {
                ConAfterimage afterimage = (ConAfterimage)targetConditions[typeof(ConAfterimage)].Single();
                dmg = 0;
                afterimage.Mod(-1);
                if (afterimage.value <= 0) afterimage.Kill();
            }

            if (originChara != null)
            {
                if (target.HasElement(Constants.FeatAegisId) && target.pos.Distance(originChara.pos) > 1)
                {
                    // Aegis - 30% Chance to halve incoming ranged damage
                    if (EClass.rnd(3) == 0)
                    {
                        target.Say("aegis_activation".langGame(target.NameSimple));
                        target.PlaySound("parry");
                        if (!HelperFunctions.NihilActivated(originChara))
                        {
                            dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 0.5F);
                        }
                    }
                }
                else if (target.HasElement(Constants.FeatAegisPlusId) && target.pos.Distance(originChara.pos) > 1)
                {
                    // Aegis+ - 100% chance to halve incoming ranged damage. Enemy Only.
                    target.Say("aegis_activation".langGame(target.NameSimple));
                    target.PlaySound("parry");
                    if (!HelperFunctions.NihilActivated(originChara))
                    {
                        dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 0.5F);
                    }
                }
                else if (target.HasElement(Constants.FeatPaviseId) && target.pos.Distance(originChara.pos) > 1)
                {
                    // Pavise - 30% Chance to halve incoming close ranged damage
                    if (EClass.rnd(3) == 0)
                    {
                        target.Say("pavise_activation".langGame(target.NameSimple));
                        target.PlaySound("parry");
                        if (!HelperFunctions.NihilActivated(originChara))
                        {
                            dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 0.5F);
                        }
                    }
                }
                else if (target.HasElement(Constants.FeatPavisePlusId) && target.pos.Distance(originChara.pos) > 1)
                {
                    // Pavise+ - 100% chance to halve incoming close ranged damage.
                    target.Say("pavise_activation".langGame(target.NameSimple));
                    target.PlaySound("parry");
                    if (!HelperFunctions.NihilActivated(originChara))
                    {
                        dmg = CardDamageHPPatches.ApplyDamageMultiplier(dmg, 0.5F);
                    }
                }

                // Sol - 30% chance to heal 50% of the damage dealt as HP.
                if (originChara.HasElement(Constants.FeatSolId) && EClass.rnd(3) == 0)
                {
                    originChara.Say("sol_activation".langGame(target.NameSimple));
                    originChara.PlaySound("warcry");
                    if (!HelperFunctions.NihilActivated(target))
                    {
                        int heal = (int)(dmg * 0.5F);
                        originChara.HealHP(heal, HealSource.Magic);
                    }
                }
            }

            /* Moved these into the Transpiler to trigger post Damage Reduction.
            // Protection - Protects flat amount of damage.
            if (targetConditions.Contains(typeof(ConProtection)))
            {
                ConProtection protection = (ConProtection)targetConditions[typeof(ConProtection)].Single();
                if (protection.value >= dmg)
                {
                    protection.Mod((int)(0 - dmg));
                    return false;
                }

                dmg -= protection.value;
                protection.Kill();
            }

            // Mana Shield - Protects a flat amount of damage with shield gating.
            // Taking any hit will reset the cooldown delay though.
            if (targetConditions.Contains(typeof(StanceManaShield)))
            {
                StanceManaShield manaShield = (StanceManaShield)targetConditions[typeof(StanceManaShield)].Single();
                if (manaShield.Stacks > 0)
                {
                    manaShield.ModShield((int)(0 - dmg));
                    dmg = 0;
                }
            }

            // Witch Hunter - When HP damage is done as a Witch Hunter with Melee/Ranged, they will also inflict 10% of the damage as mana.
            if (originChara != null && originChara.MatchesPromotion(Constants.FeatWitchHunter && attackSource is AttackSource.Melee or AttackSource.Range && dmg > 0)
            {
                int manaDamage = (int)(dmg * 0.1F) * -1;
                target.mana.Mod(manaDamage);
            }
            */
        }
        return true;
    }

    [HarmonyPatch(nameof(Card.DamageHP), typeof(long), typeof(int), typeof(int), typeof(AttackSource), typeof(Card), typeof(bool), typeof(Thing), typeof(Chara))]
    [HarmonyTranspiler]
    internal static IEnumerable<CodeInstruction> OnDamageHP_TranspilePatch(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        Type? displayClassType = typeof(Card)
                .GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Public)
                .FirstOrDefault(t => t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Any(f => f.Name == "dmg" && f.FieldType == typeof(long)));
        if (displayClassType == null) throw new Exception("Why was this removed?");
        FieldInfo? dmgField = displayClassType.GetField("dmg", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        MethodInfo secondaryDamageReduction = AccessTools.Method(
            typeof(CardDamageHPPatches),
            nameof(CardDamageHPPatches.ApplyDamageReduction),
            new[]
            {
                typeof(Card),
                typeof(Card),
                typeof(long),
                typeof(AttackSource)
            });

        MethodInfo additonalResistancePierce = AccessTools.Method(
            typeof(CardDamageHPPatches),
            nameof(CardDamageHPPatches.CalculateAdditionalResistancePierce),
            new[]
            {
                typeof(Card),
                typeof(Card)
            }
        );

        MethodInfo getResistDamage = AccessTools.Method(
            typeof(Element), "GetResistDamage",
            new[]
            {
                typeof(int),
                typeof(int),
                typeof(int)
            }
        );

        // We'll add this to the piercing power after.
        LocalBuilder pierceLocal = il.DeclareLocal(typeof(int));

        CodeMatcher matcher = new CodeMatcher(instructions, il)
                // Apply Battlemage Pierce if needed.
                .MatchForward(false, new CodeMatch(OpCodes.Call, getResistDamage))
                .Advance(-1)
                .Insert(
                    new CodeInstruction(OpCodes.Ldarg, 5), // origin card
                    new CodeInstruction(OpCodes.Ldarg_0), // __instance
                    new CodeInstruction(OpCodes.Call, additonalResistancePierce),
                    new CodeInstruction(OpCodes.Stloc, pierceLocal),
                    new CodeInstruction(OpCodes.Ldloc, pierceLocal),
                    new CodeInstruction(OpCodes.Add)
                )
                // Apply Secondary Damage Reduction
                .MatchForward(false,
                    new CodeMatch(OpCodes.Ldloc_0),
                    new CodeMatch(OpCodes.Ldfld, dmgField),
                    new CodeMatch(OpCodes.Ldc_I4, 99999999),
                    new CodeMatch(OpCodes.Conv_I8),
                    new CodeMatch(ci => ci.opcode == OpCodes.Ble || ci.opcode == OpCodes.Ble_S))
                .Advance(1)
                .Insert(
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldarg_S, 5),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldfld, dmgField),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Call, secondaryDamageReduction),
                    new CodeInstruction(OpCodes.Stfld, dmgField)
                );
        return matcher.InstructionEnumeration();
    }

    internal static int CalculateAdditionalResistancePierce(Card origin, Card target)
    {
        // Battlemages in Focus Stance with mana remaining will pierce one tier.
        int additionalPierce = 0;
        if (origin != null && origin.Chara.MatchesPromotion(Constants.FeatBattlemage) && origin.HasCondition<StanceManaFocus>() && origin.Chara.mana.value > 0)
        {
            additionalPierce++;
        }

        // Luna will pierce an additional tier of resistances.
        if (target.HasCondition<ConLuna>())
        {
            additionalPierce++;
        }
        return additionalPierce;
    }

    internal static long ApplyDamageReduction(Card origin, Card target, long damage, AttackSource source)
    {
        Chara targetChara = target.Chara;
        if (target.Chara == null) return damage;
        ILookup<Type, Condition> targetConditions = targetChara.conditions.ToLookup(c => c.GetType());
        Chara? originChara = origin?.Chara;

        long damageWithMods = damage;

        if ((uint)(source - 3) > 2u && (uint)(source - 13) > 4u)
        {
            if (targetConditions.Contains(typeof(ConProtection)))
            {
                ConProtection protection = (ConProtection)targetConditions[typeof(ConProtection)].Single();
                if (protection.value >= damage)
                {
                    protection.Mod((int)(0 - damage));
                    damageWithMods = 0;
                }
                else
                {
                    damageWithMods -= protection.value;
                    protection.Kill();
                }
            }

            // Mana Shield - Protects a flat amount of damage with shield gating.
            // Taking any hit will reset the cooldown delay though.
            if (targetConditions.Contains(typeof(StanceManaShield)))
            {
                StanceManaShield manaShield = (StanceManaShield)targetConditions[typeof(StanceManaShield)].Single();
                if (manaShield.Stacks > 0)
                {
                    manaShield.ModShield((int)(0 - damage));
                    damageWithMods = 0;
                }
            }

            // Witch Hunter - When HP damage is done as a Witch Hunter with Melee/Ranged, they will also inflict 10% of the damage as mana.
            if (originChara != null &&
                originChara.MatchesPromotion(Constants.FeatWitchHunter) &&
                source is AttackSource.Melee or AttackSource.Range &&
                damageWithMods > 0)
            {
                int manaDamage = (int)(damageWithMods * 0.1F) * -1;
                targetChara.mana.Mod(manaDamage);
            }

            // If the target has Mana Leak, restore 10% of the damage done as mana.
            if (targetConditions.Contains(typeof(ConManaLeak)) && originChara != null)
            {
                int manaSteal = (int)(damageWithMods * 0.1F);
                originChara.mana.Mod(manaSteal);
            }
        }

        return damageWithMods;
    }

    private static long ApplyDamageMultiplier(long damage, float multiplier)
    {
        return (long)(damage * multiplier);
    }
}