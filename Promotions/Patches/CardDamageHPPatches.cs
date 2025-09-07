using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
using PromotionMod.Stats.Harbinger;
using PromotionMod.Stats.Luminary;
using PromotionMod.Stats.Runeknight;
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

    [HarmonyPatch(nameof(Card.DamageHP), typeof(int), typeof(int), typeof(int), typeof(AttackSource), typeof(Card), typeof(bool), typeof(Thing), typeof(Chara))]
    [HarmonyPrefix]
    internal static bool OnDamageHP(Card __instance, ref int dmg, ref int ele, ref int eleP, AttackSource attackSource, Card origin, bool showEffect, Thing weapon)
    {
        if (__instance.isChara && ActiveAttackSources.Contains(attackSource))
        {
            Chara target = __instance.Chara;
            Chara originChara = origin.Chara;

            // Build condition dictionaries for fast lookup
            Dictionary<Type, Condition> targetConditions = target.conditions.GroupBy(c => c.GetType()).ToDictionary(g => g.Key, g => g.First());
            Dictionary<Type, Condition>? originConditions = originChara?.conditions?.GroupBy(c => c.GetType()).ToDictionary(g => g.Key, g => g.First());

            float damageMultiplier = 1F;
            List<Chara> targetAllies = HelperFunctions.GetCharasWithinRadius(target.pos, 5f, target, true, false);

            // Harbinger - Every active Miasma on the target boosts damage from Harbingers by 5%.
            if (targetConditions.ContainsKey(typeof(ConHarbingerMiasma)) && origin.isChara && origin.Evalue(Constants.FeatHarbinger) > 0)
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

            // Hexer - When taking damage, there is a 10% chance to force apply a hex out of a pool.
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
            if (!targetConditions.ContainsKey(typeof(StVanguardStance)) && targetAllies.Count(c => c.conditions.Any(cond => cond.GetType() == typeof(StVanguardStance))) > 0)
            {
                Chara luminaryAlly = targetAllies.First(c => c.conditions.Any(cond => cond.GetType() == typeof(StVanguardStance)));
                luminaryAlly.DamageHP(dmg, ele, eleP, attackSource, origin, showEffect, weapon);
                return false;
            }

            // Luminary - When taking damage, if currently Parrying, reduce damage to zero.
            if (targetConditions.ContainsKey(typeof(ConParry)))
            {
                dmg = 0;
                target.AddCooldown(Constants.ActParryId, -5);
            }

            // Luminary - Reduces damage based on Class Condition Stacks (1% per stack, caps at 30)
            if (targetConditions.ContainsKey(typeof(ConLuminary)))
            {
                ConLuminary luminary = targetConditions.GetValueOrDefault(typeof(ConLuminary)) as ConLuminary;
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
            if (targetConditions.ContainsKey(typeof(ConElementalAttunement)))
            {
                ConElementalAttunement attunement = targetConditions.GetValueOrDefault(typeof(ConElementalAttunement)) as ConElementalAttunement;
                if (attunement != null)
                {
                    attunement.StoredDamage += dmg;
                    dmg = 0;
                    return false;
                }
            }

            // Rune Knight - Runic Guard is removed and Elemental Attunement is added. All damage is absorbed.
            if (targetConditions.ContainsKey(typeof(ConRunicGuard)) && ele != Constants.EleVoid)
            {
                target.RemoveCondition<ConRunicGuard>();
                ConElementalAttunement attunement = target.AddCondition<ConElementalAttunement>() as ConElementalAttunement;
                if (attunement != null)
                {
                    attunement.AttunedElement = ele;
                }

                // Absorbing up to 20% of the damage as mana and stamina.
                int restorationAmount = Math.Min(dmg / 5, 20);
                target.mana.Mod(restorationAmount);
                target.stamina.Mod(restorationAmount);
                return false;
            }

            // Rune Knight - Warding runes will reduce incoming damage by 20% in exchange for a charge.
            if (targetConditions.ContainsKey(typeof(ConWardingRune)))
            {
                targetConditions[typeof(ConWardingRune)].Mod(-1);
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

            // Sovereign - Law Stance Reduces damage by 10%.
            if (targetConditions.ContainsKey(typeof(ConSovereignLaw)))
            {
                damageMultiplier -= 0.1F;
            }
            // Sovereign - Chaos Stance Increases damage by 10%.
            if (originConditions != null && originConditions.ContainsKey(typeof(ConSovereignLaw)))
            {
                damageMultiplier += 0.1F;
            }

            // Sovereign - Barricade Order : Reduces damage based on # of allies neighboring you with OrderBarricade
            // 5% damage reduced per ally in coherence.
            if (targetConditions.ContainsKey(typeof(ConOrderBarricade)))
            {
                float barricadeCoherence = target.pos.ListCharasInNeighbor(delegate(Chara c)
                                         {
                                             if (c == Act.CC || c.IsHostile(Act.CC) || !c.conditions.Any(cond => cond.GetType() == typeof(ConOrderBarricade)))
                                             {
                                                 return true;
                                             }
                                             return false;
                                         }).Count *
                                         0.05F;

                damageMultiplier -= barricadeCoherence;
            }

            // Spellblade - If the Spellblade is using Siphoning Blade. do no damage and instead deal the 50% damage as MP instead, absorbing it.
            if (origin.Chara.Evalue(Constants.FeatSpellblade) > 0)
            {
                if (originConditions != null && originConditions.ContainsKey(typeof(ConSiphoningBlade)))
                {
                    dmg = (int)(dmg * 0.5F);
                    target.mana.Mod(0 - dmg);
                    origin.Chara.mana.Mod(dmg);
                    return false;
                }
            }

            // Spellblade and Elementalist - Excel at applying status effects. eleP doubled.
            if (origin.Chara.Evalue(Constants.FeatSpellblade) > 0 || origin.Chara.Evalue(Constants.FeatElementalist) > 0)
            {
                eleP = HelperFunctions.SafeMultiplier(eleP, 2);
            }


            // War Cleric - Sol Blade causes the healer to absorb 30% of the damage dealt.
            if (originChara != null && originChara.Evalue(Constants.FeatWarCleric) > 0 && originConditions != null && originConditions.ContainsKey(typeof(ConSolBlade)))
            {
                int heal = (int)(dmg * 0.3F);
                originChara.HealHP(heal, HealSource.Magic);
            }

            // War Cleric - Sanctuary reduces all damage dealt by 75%.
            if (targetConditions.ContainsKey(typeof(ConSanctuary)))
            {
                damageMultiplier -= 0.75F;
            }

            // Protection - Protects flat amount of damage.
            if (targetConditions.ContainsKey(typeof(ConProtection)))
            {
                ConProtection protection = targetConditions.GetValueOrDefault(typeof(ConProtection)) as ConProtection;
                if (protection.value >= dmg)
                {
                    protection.Mod(-1 * dmg);
                    return false;
                }

                dmg -= protection.value;
                protection.Kill();
            }
        }
        return true;
    }
}