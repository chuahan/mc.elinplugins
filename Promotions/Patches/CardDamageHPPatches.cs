using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
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
    internal static List<AttackSource> ActiveAttackSources = new List<AttackSource>
    {
        AttackSource.Melee,
        AttackSource.Range,
        AttackSource.Throw,
        AttackSource.MagicSword,
        AttackSource.Shockwave,
        AttackSource.None
    };

    [HarmonyPatch(nameof(Card.DamageHP), typeof(long), typeof(int), typeof(int), typeof(AttackSource), typeof(Card), typeof(bool), typeof(Thing), typeof(Chara))]
    [HarmonyPrefix]
    internal static bool OnDamageHP_Patches(Card __instance, ref long dmg, ref int ele, ref int eleP, AttackSource attackSource, Card origin, bool showEffect, Thing weapon)
    {
        if (__instance.isChara && ActiveAttackSources.Contains(attackSource))
        {
            Chara target = __instance.Chara;
            Chara originChara = origin.Chara;

            // Build condition lookups for perf.
            ILookup<Type, Condition> targetConditions = target.conditions.ToLookup(c => c.GetType());
            ILookup<Type, Condition> originConditions = originChara.conditions.ToLookup(c => c.GetType());

            float damageMultiplier = 1F;
            List<Chara> targetAllies = HelperFunctions.GetCharasWithinRadius(target.pos, 5f, target, true, false);

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
            if (targetConditions.Contains(typeof(ConHarbingerMiasma)) && origin.isChara && origin.Evalue(Constants.FeatHarbinger) > 0)
            {
                int miasmaCount = target.conditions.Count(con => con is ConHarbingerMiasma);
                damageMultiplier += miasmaCount * 0.05F;
            }

            // Headhunter - Damage is increased by 25% against higher quality enemies.
            if (target.source.quality >= 3 && origin.isChara && origin.Evalue(Constants.FeatHeadhunter) > 0)
            {
                damageMultiplier += 0.25F;
            }

            // Hexer - When applying spell damage there is a 10% chance to force apply a hex out of a pool.
            if (origin.Evalue(Constants.FeatHexer) > 0 && attackSource == AttackSource.None)
            {
                if (EClass.rnd(10) == 0)
                {
                    origin.SayRaw("hexer_hextouch".lang());
                    FeatHexer.ApplyCondition(__instance.Chara, origin.Chara, 100, true);
                }
            }

            // Hexer - When taking damage, there is a 10% chance to force-apply a hex out of a pool.
            if (target.Evalue(Constants.FeatHexer) > 0 && ActiveAttackSources.Contains(attackSource))
            {
                if (EClass.rnd(10) == 0)
                {
                    origin.SayRaw("hexer_revenge".lang());
                    FeatHexer.ApplyCondition(origin.Chara, __instance.Chara, 100, true);
                }
            }

            // Luminary - Vanguard Stance: Redirect damage from allies to the Luminary in Vanguard Stance.
            // Does not redirect if the target is already the Luminary.
            if (!targetConditions.Contains(typeof(StVanguardStance)) && targetAllies.Count(c => c.conditions.Any(cond => cond.GetType() == typeof(StVanguardStance))) > 0)
            {
                Chara luminaryAlly = targetAllies.First(c => c.conditions.Any(cond => cond.GetType() == typeof(StVanguardStance)));
                luminaryAlly.DamageHP(dmg, ele, eleP, attackSource, origin, showEffect, weapon);
                return false;
            }

            // Luminary - When taking damage, if currently Parrying, reduce damage to zero.
            if (targetConditions.Contains(typeof(ConLuminousDeflection)))
            {
                dmg = 0;
                target.AddCooldown(Constants.ActLuminousDeflectionId, -5);
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

            // Saint - If the Saint and the target share the same religion, the Saint can attempt to convert the opponent.
            if (origin.Chara.Evalue(Constants.FeatSaint) > 0 && origin.Chara.faith.id == target.faith.id)
            {
                if (Math.Max(origin.Evalue(85), origin.Evalue(Constants.FaithId)) > target.Evalue(Constants.FaithId) &&
                    !target.IsMinion &&
                    target.CanBeTempAlly(origin.Chara))
                {
                    target.Say("dominate_machine", target, origin);
                    target.PlayEffect("boost");
                    target.PlaySound("boost");
                    target.ShowEmo(Emo.love);
                    target.lastEmo = Emo.angry;
                    target.Chara.MakeMinion(origin.Chara.IsPCParty ? EClass.pc : origin.Chara);
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

            // Sovereign - Law Stance Reduces damage by 10%.
            if (targetConditions.Contains(typeof(ConSovereignLaw)))
            {
                damageMultiplier -= 0.1F;
            }
            // Sovereign - Chaos Stance Increases damage by 10%.
            if (originConditions.Contains(typeof(ConSovereignChaos)))
            {
                damageMultiplier += 0.1F;
            }

            // Sovereign - Barricade Order: Reduces damage based on # of allies neighboring you
            // 5% damage reduced per ally in coherence.
            if (targetConditions.Contains(typeof(ConOrderBarricade)))
            {
                float barricadeCoherence = target.pos.ListCharasInNeighbor(c => c == Act.CC || c.IsHostile(Act.CC)).Count * 0.05F;
                damageMultiplier -= barricadeCoherence;
            }

            // Spellblade - If the Spellblade is using Siphoning Blade. do no damage and instead deal the 50% damage as MP instead, absorbing it.
            if (origin.Chara.Evalue(Constants.FeatSpellblade) > 0)
            {
                if (originConditions.Contains(typeof(ConSiphoningBlade)))
                {
                    dmg = (int)(dmg * 0.5F);
                    target.mana.Mod((int)(0 - dmg));
                    origin.Chara.mana.Mod((int)dmg);
                    return false;
                }
            }

            // Spellblade and Elementalist - Excel at applying status effects. eleP doubled.
            if (origin.Chara.Evalue(Constants.FeatSpellblade) > 0 || origin.Chara.Evalue(Constants.FeatElementalist) > 0)
            {
                eleP = HelperFunctions.SafeMultiplier(eleP, 2);
            }


            // War Cleric - Sol Blade causes the healer to absorb 30% of the damage dealt.
            if (originChara.Evalue(Constants.FeatWarCleric) > 0 && originConditions.Contains(typeof(ConSolBlade)))
            {
                int heal = (int)(dmg * 0.3F);
                originChara.HealHP(heal, HealSource.Magic);
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
                int energyPower = manaShield.Stacks;
                manaShield.ModShield((int)(0-dmg));
                if (energyPower > 0)
                {
                    dmg = 0;
                }
            }
        }
        return true;
    }

    [HarmonyPatch(nameof(Card.DamageHP), typeof(long), typeof(int), typeof(int), typeof(AttackSource), typeof(Card), typeof(bool), typeof(Thing), typeof(Chara))]
    [HarmonyTranspiler]
    internal IEnumerable<CodeInstruction> OnDamageHP_TranspilePatch(IEnumerable<CodeInstruction> instructions)
    {
        MethodInfo damagePierceMethod = AccessTools.Method(typeof(CardDamageHPPatches), nameof(CardDamageHPPatches.BattleMageDamagePierce));
        FieldInfo origin = AccessTools.Field(typeof(Card), "origin");

        CodeMatcher matcher = new CodeMatcher(instructions)
                .MatchForward(true,
                    new CodeMatch(OpCodes.Ldloc_S), // power
                    new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Element), "GetResistDamage"))
                )
                .ThrowIfInvalid("GetResistDamage not found")
                .Advance(-1)
                .Insert(
                    // Pass Origin Card into the Damage Pierce
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldfld, origin),
                    // Call DamagePiercePatch(origin)
                    new CodeInstruction(OpCodes.Call, damagePierceMethod),
                    // Add to power
                    new CodeInstruction(OpCodes.Add)
                ); 
        
        return matcher.InstructionEnumeration();

    }

    internal int BattleMageDamagePierce(Chara origin)
    {
        // Battlemages in Focus Stance with mana remaining will pierce one tier.
        if (origin.Evalue(Constants.FeatBattlemage) > 0 && origin.HasCondition<StanceManaFocus>() && origin.mana.value > 0)
        {
            return 1;
        }
        return 0;
    }
}