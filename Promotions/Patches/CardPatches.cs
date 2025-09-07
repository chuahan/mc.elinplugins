using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.Headhunter;
using PromotionMod.Stats.Necromancer;
using PromotionMod.Stats.Sentinel;
using PromotionMod.Stats.Sovereign;
using PromotionMod.Stats.Trickster;
using PromotionMod.Trait.Trickster;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Card))]
public class CardPatches
{
    [HarmonyPatch(nameof(Card.ModExp))]
    [HarmonyPrefix]
    internal static bool AdventurerDoubleExp(Card __instance, int ele, ref int a)
    {
        if (__instance.isChara)
        {
            if (__instance.Chara.IsPCFaction && EClass.pc.Evalue(Constants.FeatAdventurer) > 0)
            {
                // Adventurers increase the exp gain by 50%
                a = (int)(a * 1.5F);
            }
        }
        return true;
    }

    [HarmonyPatch(typeof(Card), "get_PV")]
    [HarmonyPostfix]
    internal static void PvPatches(Card __instance, ref int __result)
    {
        // Battlemage - Mana Armor
        if (__instance.isChara)
        {
            Chara chara = __instance.Chara;
            if (chara.Evalue(Constants.FeatBattlemage) > 0)
            {
                int manaValue = chara.mana.max / 5;
                __result += manaValue;
            }

            // Sentinel
            if (chara.Evalue(Constants.FeatSentinel) > 0)
            {
                // Double PV if Heavy Armor + Shield.
                if (chara.GetArmorSkill() == 122 && chara.body.GetAttackStyle() == AttackStyle.Shield)
                {
                    __result = HelperFunctions.SafeMultiplier(__result, 2);
                }

                // PV is set to 0 when in Rage Stance.
                if (chara.HasCondition<StanceRage>())
                {
                    __result = 0;
                }
            }
        }
    }

    [HarmonyPatch(nameof(Card.Die))]
    [HarmonyPrefix]
    internal static bool OnDieOverloads(Card __instance, Element e, Card origin, AttackSource attackSource)
    {
        if (__instance.isChara)
        {
            Chara target = __instance.Chara;
            // Berserker - Heal on Kill
            if (origin.isChara && origin.Chara.Evalue(Constants.FeatBerserker) > 0)
            {
                int healAmount = (int)(origin.Chara.MaxHP * .25F);
                origin.Say("berserker_revel".langGame(origin.Chara.NameSimple));
                origin.Chara.HealHP(healAmount);
            }

            // Headhunter - Gain Headhunter stacks on Kill.
            if (origin.isChara && origin.Chara.Evalue(Constants.FeatHeadhunter) > 0)
            {
                if (!origin.Chara.HasCondition<ConHeadhunter>())
                {
                    origin.Chara.AddCondition<ConHeadhunter>(1);
                }
                else
                {
                    int newStacks = origin.Chara.GetCondition<ConHeadhunter>().power + 1;
                    origin.Chara.AddCondition<ConHeadhunter>(newStacks);
                }
            }

            // Necromancer - If target is afflicted with ConDeadBeckon, on death will summon a Death Knight
            if (target.HasCondition<ConDeadBeckon>())
            {
                ConDeadBeckon deadBeckon = target.GetCondition<ConDeadBeckon>();
                Chara necromancer = EClass._map.zone.FindChara(deadBeckon.NecromancerUID);

                Chara summon = CharaGen.Create(Constants.NecromancerDeathKnightCharaId);
                summon.isSummon = true;
                summon.SetLv(target.LV);
                summon.interest = 0;
                necromancer.currentZone.AddCard(summon, __instance.pos);
                summon.PlayEffect("mutation");
                summon.MakeMinion(necromancer);

                // Equip the Death Knight with full heavy armor + a sword.
                summon.AddThing(ThingGen.Create("sword", 40, summon.LV));
                summon.AddThing(ThingGen.Create("shield_knight", 40, summon.LV));
                summon.AddThing(ThingGen.Create("helm_knight", 40, summon.LV));
                summon.AddThing(ThingGen.Create("armor_breast", 40, summon.LV));
                summon.AddThing(ThingGen.Create("boots_heavy", 40, summon.LV));
            }

            // Sovereign - Rout Order will replenish value on kill. Any active Intonation will also replenish value.
            if (origin.isChara)
            {
                origin.Chara.GetCondition<ConOrderRout>()?.Mod(1);
                origin.Chara.GetCondition<ConWeapon>()?.Mod(1);
            }

            // Trickster - Phantom Trickster Ids will inflict one of the random Trickster debuffs on their killer.
            if (origin.isChara && origin.Chara.IsHostile(target) && target.id == Constants.PhantomTricksterCharaId)
            {
                string randomCondition = TraitTricksterArcaneTrap.TricksterDebuffs.RandomItem();
                Condition tricksterCondition = Condition.Create(randomCondition, target.LV);
                origin.Chara.AddCondition(tricksterCondition, true);
            }
        }
        return true;
    }

    [HarmonyPatch(nameof(Card.HealHP))]
    [HarmonyPrefix]
    internal static bool OnHealHP(Card __instance, int a, HealSource origin)
    {
        // Trickster - Despair Debuff prevents healing.
        if (__instance.isChara)
        {
            Chara chara = __instance.Chara;
            // Build condition dictionaries for fast lookup
            Dictionary<Type, Condition> targetConditions = chara.conditions.GroupBy(c => c.GetType()).ToDictionary(g => g.Key, g => g.First());

            if (targetConditions.ContainsKey(typeof(ConDespair)))
            {
                chara.Say("trickster_despair_nullheal".lang());
                return false;
            }

            // War Cleric - Healing is copied to nearby allies with 50% efficacy.
            // Does not trigger on HealSource.None, which is Regen, or duplicated healing, or mods who don't actually add a HealSource.
            if (__instance.Chara.Evalue(Constants.FeatWarCleric) > 0 && origin != HealSource.None)
            {
                chara.Say("warcleric_healshare".lang());
                foreach (Chara ally in HelperFunctions.GetCharasWithinRadius(__instance.pos, 3F, __instance.Chara, true, false))
                {
                    ally.HealHP(a / 2);
                }
            }
        }

        return true;
    }
}