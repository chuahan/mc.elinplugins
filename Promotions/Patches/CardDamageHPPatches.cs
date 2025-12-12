using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
using PromotionMod.Stats.Artificer;
using PromotionMod.Stats.Battlemage;
using PromotionMod.Stats.Harbinger;
using PromotionMod.Stats.Luminary;
using PromotionMod.Stats.Runeknight;
using PromotionMod.Stats.Sniper;
using PromotionMod.Stats.Sovereign;
using PromotionMod.Stats.Spellblade;
using PromotionMod.Stats.WarCleric;
using StVanguardStance = PromotionMod.Stats.Luminary.StVanguardStance;
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
        AttackSource.None,
    };

    [HarmonyPatch(nameof(Card.DamageHP), typeof(long), typeof(int), typeof(int), typeof(AttackSource), typeof(Card), typeof(bool), typeof(Thing), typeof(Chara))]
    [HarmonyPrefix]
    internal static bool OnDamageHP_Patches(Card __instance, ref long dmg, ref int ele, ref int eleP, AttackSource attackSource, Card? origin, bool showEffect, Thing weapon, Chara originalTarget)
    {
        //if (__instance.isChara && ActiveAttackSources.Contains(attackSource))
        // This is the same attack source conditions as Wall of Flesh, so active attack sources.
        if (__instance.isChara && ((uint)(attackSource - 3) > 2u && (uint)(attackSource - 13) > 4u))
        {
            Chara target = __instance.Chara;
            Chara? originChara = origin?.Chara;

            float damageMultiplier = 1F;
            
            // Target Conditionals
            ILookup<Type, Condition> targetConditions = target.conditions.ToLookup(c => c.GetType());

            // Origin Conditionals
            if (originChara != null)
            {
                ILookup<Type, Condition>? originConditions = originChara.conditions.ToLookup(c => c.GetType());
                
                // Artificer - If the target is riding a Titan Golem, divert damage to the Titan Golem instead.
                if (target.ride is { id: Constants.TitanGolemCharaId })
                {
                    target.ride.DamageHP(dmg, ele, eleP, attackSource, origin, showEffect, weapon, target);
                    return false;
                }
                
                // Hexer - When applying spell damage, there is a 10% chance to apply a hex out of a pool.
                if (originChara.Evalue(Constants.FeatHexer) > 0 && MagicAttackSources.Contains(attackSource))
                {
                    if (EClass.rnd(10) == 0)
                    {
                        originChara.SayRaw("hexer_hextouch".lang());
                        FeatHexer.ApplyCondition(__instance.Chara, originChara, 100, true);
                    }
                }

                // Hexer - When taking damage, there is a 10% chance to force-apply a hex out of a pool in retaliation.
                if (target.Evalue(Constants.FeatHexer) > 0)
                {
                    if (EClass.rnd(10) == 0)
                    {
                        target.SayRaw("hexer_revenge".lang(originChara.NameSimple));
                        FeatHexer.ApplyCondition(originChara, __instance.Chara, 100, true);
                    }
                }
                
                // Maia - Maias deal 50% increased damage against their opposing element(s).
                if (originChara.Evalue(Constants.FeatMaiaEnlightened) > 0 && target.source.mainElement.Contains(Constants.ElementAliasLookup[Constants.EleDarkness].Remove(0, 3)))
                {
                    damageMultiplier += 0.5F;
                }

                if (originChara.Evalue(Constants.FeatMaiaCorrupted) > 0 && target.source.mainElement.Contains(Constants.ElementAliasLookup[Constants.EleHoly].Remove(0, 3)))
                {
                    damageMultiplier += 0.5F;
                }

                // Harbinger - Every active Miasma on the target boosts damage from Harbingers by 5%.
                if (originChara.Evalue(Constants.FeatHarbinger) > 0 && targetConditions.Contains(typeof(ConHarbingerMiasma)))
                {
                    int miasmaCount = target.conditions.Count(con => con is ConHarbingerMiasma);
                    damageMultiplier += miasmaCount * 0.05F;
                }

                // Headhunter - Damage is increased by 25% against higher quality enemies.
                if (target.source.quality >= 3 && originChara.Evalue(Constants.FeatHeadhunter) > 0)
                {
                    damageMultiplier += 0.25F;
                }
                
                // Saint - If the Saint and the target share the same religion, the Saint can attempt to convert the opponent.
                if (originChara.Evalue(Constants.FeatSaint) > 0 && originChara.faith.id == target.faith.id)
                {
                    if (Math.Max(originChara.Evalue(85), originChara.Evalue(Constants.FaithId)) > target.Evalue(Constants.FaithId) &&
                        !target.IsMinion &&
                        target.CanBeTempAlly(originChara))
                    {
                        target.Say("dominate_machine", target, originChara);
                        target.PlayEffect("boost");
                        target.PlaySound("boost");
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
                    damageMultiplier += 0.1F;
                }

            
                // Spellblade - If the Spellblade is using Siphoning Blade. Do no damage and instead deal the 50% damage as MP instead, absorbing it.
                if (originChara.Evalue(Constants.FeatSpellblade) > 0)
                {
                    if (originConditions.Contains(typeof(ConSiphoningBlade)))
                    {
                        dmg = (int)(dmg * 0.5F);
                        target.mana.Mod((int)(0 - dmg));
                        originChara.mana.Mod((int)dmg);
                        return false;
                    }
                }
            
                // War Cleric - Sol Blade causes the healer to absorb 30% of the damage dealt.
                if (originChara.Evalue(Constants.FeatWarCleric) > 0 && originConditions.Contains(typeof(ConSolBlade)))
                {
                    int heal = (int)(dmg * 0.3F);
                    originChara.HealHP(heal, HealSource.Magic);
                }
                // Spellblade and Elementalist - Excel at applying status effects. eleP doubled.
                if (originChara.Evalue(Constants.FeatSpellblade) > 0 || originChara.Evalue(Constants.FeatElementalist) > 0)
                {
                    eleP = HelperFunctions.SafeMultiplier(eleP, 2);
                }
            }

            // Artificer - Heavenly Pearl will decrease a damage instance by 25%
            if (targetConditions.Contains(typeof(ConHeavenlyEmbrace)))
            {
                ConHeavenlyEmbrace heavenlyEmbrace = (ConHeavenlyEmbrace)targetConditions[typeof(ConHeavenlyEmbrace)].Single();
                damageMultiplier -= 0.25F;
                heavenlyEmbrace.Mod(-1);
            }

            // Luminary - Vanguard Stance: Redirect damage from allies to the Luminary in Vanguard Stance.
            // Does not redirect if the target is already the Luminary.
            if (!targetConditions.Contains(typeof(StVanguardStance)))
            {
                List<Chara> targetAllies = HelperFunctions.GetCharasWithinRadius(target.pos, 5f, target, true, false);
                if (targetAllies.Count(c => c.conditions.Any(cond => cond.GetType() == typeof(StVanguardStance))) > 0)
                {
                    Chara luminaryAlly = targetAllies.First(c => c.conditions.Any(cond => cond.GetType() == typeof(StVanguardStance)));
                    luminaryAlly.DamageHP(dmg, ele, eleP, attackSource, origin, showEffect, weapon, target);
                    return false;   
                }
            }

            // Luminary - When taking damage, if currently Parrying, reduce damage to zero and add a stack of Luminary.
            if (targetConditions.Contains(typeof(ConLuminousDeflection)))
            {
                dmg = 0;
                target.AddCooldown(Constants.ActLuminousDeflectionId, -5);
                ConLuminary? luminary = (ConLuminary)targetConditions[typeof(ConLuminary)].Single() ?? target.AddCondition<ConLuminary>() as ConLuminary;
                luminary?.AddStacks(1);
            }

            // Luminary - Reduces damage based on Class Condition Stacks (1% per stack, caps at 30)
            if (targetConditions.Contains(typeof(ConLuminary)))
            {
                ConLuminary luminary = (ConLuminary)targetConditions[typeof(ConLuminary)].Single();
                damageMultiplier -= luminary.GetStacks() * 0.01F;

            }

            // Necromancer - Bone Armor. Reduces damage based on how many Skeleton Minions you have.
            // Caps at 75% damage reduction
            if (target.Evalue(Constants.FeatNecromancer) > 0)
            {
                int boneArmyCount = __instance.Chara.currentZone.ListMinions(__instance.Chara).Count(c => c.HasTag(CTAG.undead));
                damageMultiplier -= Math.Min(0.75F, boneArmyCount * 0.025F);
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
                damageMultiplier -= 0.2F;
            }
            
            // Sovereign - Law Stance Reduces damage by 10%.
            if (targetConditions.Contains(typeof(ConSovereignLaw)))
            {
                damageMultiplier -= 0.1F;
            }

            // Sovereign - Barricade Order: Reduces damage based on # of allies neighboring you
            // 5% damage reduced per ally in coherence.
            if (targetConditions.Contains(typeof(ConOrderBarricade)))
            {
                float barricadeCoherence = target.pos.ListCharasInNeighbor(c => c == Act.CC || c.IsHostile(Act.CC)).Count * 0.05F;
                damageMultiplier -= barricadeCoherence;
            }
            
            // War Cleric - Sanctuary reduces all damage dealt by 75%.
            if (targetConditions.Contains(typeof(ConSanctuary)))
            {
                damageMultiplier -= 0.75F;
            }

            // Floor Damage Multiplier to 0. Don't want any healing on negative multiplier shenanigans
            damageMultiplier = Math.Max(damageMultiplier, 0);
            // Apply Damage multiplier.
            dmg = (long)(dmg * damageMultiplier);

            // Afterimage - Fully negates one instance of damage.
            if (targetConditions.Contains(typeof(ConAfterimage)) && dmg != 0)
            {
                ConAfterimage afterimage = (ConAfterimage)targetConditions[typeof(ConAfterimage)].Single();
                dmg = 0;
                afterimage.Mod(-1);
                if (afterimage.value <= 0) afterimage.Kill();
            }

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
            if (originChara != null && originChara.Evalue(Constants.FeatWitchHunter) > 0 && attackSource is AttackSource.Melee or AttackSource.Range && dmg > 0)
            {
                int manaDamage = (int)(dmg * 0.1F) * -1;
                target.mana.Mod(manaDamage);
            }
        }
        return true;
    }

    [HarmonyPatch(nameof(Card.DamageHP), typeof(long), typeof(int), typeof(int), typeof(AttackSource), typeof(Card), typeof(bool), typeof(Thing), typeof(Chara))]
    [HarmonyTranspiler]
    internal static IEnumerable<CodeInstruction> OnDamageHP_TranspilePatch(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        // Helper: static int BattleMageDamagePierce(Card)
        MethodInfo battleMageDamagePierce = AccessTools.Method(
            typeof(CardDamageHPPatches),
            nameof(CardDamageHPPatches.BattleMageDamagePierce),
            new[] { typeof(Card) }
        );
        
        MethodInfo getResistDamage = AccessTools.Method(
            typeof(Element), "GetResistDamage",
            new[] { typeof(int), typeof(int), typeof(int) }
        );
        
        // We'll add this to the piercing power after.
        LocalBuilder pierceLocal = il.DeclareLocal(typeof(int));

        CodeMatcher m = new CodeMatcher(instructions, il)
                .MatchForward(true, new CodeMatch(OpCodes.Call, getResistDamage))
                .ThrowIfInvalid("GetResistDamage call not found")
                .Advance(-1)
                .Insert(
                    new CodeInstruction(OpCodes.Ldarg, 5),
                    new CodeInstruction(OpCodes.Call, battleMageDamagePierce),
                    new CodeInstruction(OpCodes.Stloc, pierceLocal),
                    new CodeInstruction(OpCodes.Ldloc, pierceLocal),
                    new CodeInstruction(OpCodes.Add)
                );

        return m.InstructionEnumeration();

    }

    internal static int BattleMageDamagePierce(Card origin)
    {
        // Battlemages in Focus Stance with mana remaining will pierce one tier.
        if (origin != null && origin.Evalue(Constants.FeatBattlemage) > 0 && origin.HasCondition<StanceManaFocus>() && origin.Chara.mana.value > 0)
        {
            return 1;
        }
        return 0;
    }
}