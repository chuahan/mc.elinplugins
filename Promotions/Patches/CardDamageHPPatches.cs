using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities.Hexer;
using PromotionMod.Elements.PromotionAbilities.Luminary;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
using PromotionMod.Stats.Berserker;
using PromotionMod.Stats.Harbinger;
using PromotionMod.Stats.Luminary;
using PromotionMod.Stats.Runeknight;
using PromotionMod.Stats.Sovereign;
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

    internal static MethodInfo TargetMethod()
    {
        return AccessTools.GetDeclaredMethods(typeof(Card))
                .Where(mi => mi.Name == nameof(Card.DamageHP))
                .OrderByDescending(mi => mi.GetParameters().Length)
                .First();
    }

    public static readonly FastInvokeHandler CachedInvoker = MethodInvoker.GetHandler(TargetMethod(), true);

    [HarmonyPrefix]
    internal static bool OnDamageHP(Card __instance, ref int dmg, ref int ele, ref int eleP, AttackSource attackSource, Card origin, bool showEffect, Thing weapon)
    {
        if (__instance.isChara && ActiveAttackSources.Contains(attackSource))
        {
            Chara target = __instance.Chara;
            List<Chara> targetAllies = HelperFunctions.GetCharasWithinRadius(target.pos, 5f, target, true, false);
            
            // Berserker - Berserkers under the effect of bloodlust will automatically try to retaliate against Melee Attacks
            if (target.GetCondition<ConBloodlust>() != null && attackSource == AttackSource.Melee || attackSource == AttackSource.MagicSword && origin is { isChara: true })
            {
                new ActMelee().Perform(target, origin, origin.pos);
            }

            // Harbinger - Every active Miasma on the target boosts damage from Harbingers by 5%.
            if (target.GetCondition<ConHarbingerMiasma>() != null && origin.isChara && origin.Evalue(Constants.FeatHarbinger) > 0)
            {
                int miasmaCount = target.conditions.Count(con => con is ConHarbingerMiasma);
                dmg = HelperFunctions.SafeMultiplier(dmg, (100 + miasmaCount * 5) / 100F);
            }

            // Headhunter - Damage is increased against higher quality enemies.
            if (target.source.quality >= 3 && origin.isChara && origin.Evalue(Constants.FeatHeadhunter) > 0)
            {
                dmg = HelperFunctions.SafeMultiplier(dmg, 1.25F);
            }
            
            // Hexer - When applying spell damage there is a 10% chance to force apply a hex out of a pool.
            if (origin.Evalue(Constants.FeatHexer) > 0 && attackSource == AttackSource.None)
            {
                if (EClass.rnd(10) == 0)
                {
                    origin.SayRaw("hexer_hextouch");
                    FeatHexer.ApplyCondition(__instance.Chara, origin.Chara, 100, true);
                }
            }

            // Hexer - When taking damage, there is a 10% chance to force apply a hex out of a pool.
            if (target.Evalue(Constants.FeatHexer) > 0 && ActiveAttackSources.Contains(attackSource))
            {
                if (EClass.rnd(10) == 0)
                {
                    origin.SayRaw("hexer_revenge");
                    FeatHexer.ApplyCondition(origin.Chara, __instance.Chara, 100, true);
                }
            }
            
            // Luminary - Vanguard Stance: Redirect damage from allies to the Luminary in Vanguard Stance.
            if (!target.HasCondition<StVanguardStance>() && targetAllies.Count(c => c.HasCondition<StVanguardStance>()) > 0)
            {
                Chara luminaryAlly = targetAllies.First(c => c.HasCondition<StVanguardStance>());
                luminaryAlly.DamageHP(dmg, ele, eleP, attackSource, origin, showEffect, weapon);
                return false;
            }
            
            // Luminary - When taking damage, if currently Parrying, reduce damage to zero.
            if (target.HasCondition<ConParry>())
            {
                dmg = 0;
                target.AddCooldown(Constants.ActParryId, -5);
            }
            
            // Luminary - Reduces damage based on Class Condition Stacks (1% per stack, caps at 30)
            if (target.HasCondition<ConLuminary>())
            {
                ConLuminary luminary = target.GetCondition<ConLuminary>();
                dmg = (int)(dmg * ((100 - luminary.GetStacks() * 1F) / 100));
            }
            
            // Necromancer - Bone Armor. Reduces damage based on how many Skeleton Minions you have.
            // Caps at 75% damage reduction
            if (target.Evalue(Constants.FeatNecromancer) > 0)
            {
                int boneArmyCount = __instance.Chara.currentZone.ListMinions(__instance.Chara).Count(c => c.HasTag(CTAG.undead));
                dmg = (int)(dmg * ((100 - Math.Max(75, boneArmyCount * 2.5F)) / 100));
            }
            
            // Rune Knight - Elemental Attunement. If damage received matches your attuned element, all damage is absorbed and added as stockpiled damage.
            if (target.HasCondition<ConElementalAttunement>())
            {
                ConElementalAttunement attunement = target.GetCondition<ConElementalAttunement>();
                attunement.StoredDamage += dmg;
                dmg = 0;
            }
            
            // Rune Knight - Runic Guard is removed and Elemental Attunement is added. All damage is absorbed.
            if (target.HasCondition<ConRunicGuard>() && ele != Constants.EleVoid)
            {
                target.RemoveCondition<ConRunicGuard>();
                ConElementalAttunement attunement = (target.AddCondition<ConElementalAttunement>() as ConElementalAttunement);
                if (attunement != null)
                {
                    attunement.AttunedElement = ele;
                }
                
                // Absorbing up to 20% of the damage as mana and stamina.
                int restorationAmount = Math.Min(dmg / 5, 20);
                target.mana.Mod(restorationAmount);
                target.stamina.Mod(restorationAmount);

                dmg = 0;
            }
            
            // Saint - If the Saint and the target share the same religion, the Saint can attempt to convert the opponent.
            if (origin.Chara.Evalue(Constants.FeatSaint) > 0 && origin.Chara.faith.id == target.faith.id)
            {
                if (Math.Max(origin.Evalue(85), origin.Evalue(306)) > target.Evalue(306) &&
                    (!target.IsMinion && target.CanBeTempAlly(origin.Chara)))
                {
                    target.Say("dominate_machine", target, origin);
                    target.PlayEffect("boost");
                    target.PlaySound("boost");
                    target.ShowEmo(Emo.love);
                    target.lastEmo = Emo.angry;
                    target.Chara.MakeMinion(origin.Chara.IsPCParty ? EClass.pc : origin.Chara);
                }
            }
            
            // Sovereign - Law Stance Reduces damage by 10%.
            if (target.HasCondition<ConSovereignLaw>())
            {
                dmg = (int)(dmg * 0.9F);
            }
            // Sovereign - Chaos Stance Increases damage by 10%.
            if (origin.HasCondition<ConSovereignLaw>())
            {
                dmg = (int)(dmg * 1.1F);
            }
            
            // Sovereign - Barricade Order : Reduces damage based on # of allies neighboring you with OrderBarricade
            if (target.HasCondition<ConOrderBarricade>())
            {
                float barricadeCoherence = target.pos.ListCharasInNeighbor(delegate(Chara c)
                {
                    if (c == Act.CC || c.IsHostile(Act.CC) || !c.HasCondition<ConOrderBarricade>())
                    {
                        return true;
                    }
                    return false;
                }).Count * 5;

                dmg = (int)(dmg * ((100 - barricadeCoherence) / 100F));

            }
            
            // Spellblade - Spellblades excel at applying status effects. eleP doubled.
            if (origin.Chara.Evalue(Constants.FeatSpellblade) > 0)
            {
                eleP = HelperFunctions.SafeMultiplier(eleP, 2);
            }
            
            // Protection - Protects flat amount of damage.
            if (target.HasCondition<ConProtection>())
            {
                ConProtection protection = target.GetCondition<ConProtection>();
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

