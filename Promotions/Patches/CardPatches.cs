using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.Headhunter;
using PromotionMod.Stats.Necromancer;
using PromotionMod.Stats.Sovereign;
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
    static void BattlemagePVBoostPatch(Card __instance, ref int __result)
    {
        // Battlemage - Mana Armor
        if (__instance.isChara && __instance.Chara.Evalue(Constants.FeatBattlemage) > 0)
        {
            int manaValue = __instance.Chara.mana.max / 5;
            __result += manaValue;
        }

        // Sentinel - Double PV if Heavy Armor + Shield.
        if (__instance.isChara && __instance.Chara.Evalue(Constants.FeatSentinel) > 0 && __instance.Chara.GetArmorSkill() == 122 && __instance.Chara.body.GetAttackStyle() == AttackStyle.Shield)
        {
            __result = HelperFunctions.SafeMultiplier(__result, 2);
        }
    }

    [HarmonyPatch(nameof(Card.Die))]
    [HarmonyPrefix]
    internal static bool OnDieOverloads(Card __instance, Element e, Card origin, AttackSource attackSource)
    {
        if (__instance.isChara)
        {
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
            if (__instance.HasCondition<ConDeadBeckon>())
            {
                ConDeadBeckon deadBeckon = __instance.Chara.GetCondition<ConDeadBeckon>();
                Chara necromancer = EClass._map.zone.FindChara(deadBeckon.NecromancerUID);
                
                Chara summon = CharaGen.Create(Constants.NecromancerDeathKnightCharaId);
                summon.isSummon = true;
                summon.SetLv(__instance.Chara.LV);
                summon.interest = 0;
                necromancer.currentZone.AddCard(summon, __instance.pos);
                summon.PlayEffect("mutation");
                summon.MakeMinion(necromancer);
            
                // Equip the Death Knight with full heavy armor + a sword.
                summon.AddThing(ThingGen.Create("sword", idMat: 40, lv: summon.LV));
                summon.AddThing(ThingGen.Create("shield_knight", idMat: 40, lv: summon.LV));
                summon.AddThing(ThingGen.Create("helm_knight", idMat: 40, lv: summon.LV));
                summon.AddThing(ThingGen.Create("armor_breast", idMat: 40, lv: summon.LV));
                summon.AddThing(ThingGen.Create("boots_heavy", idMat: 40, lv: summon.LV));
            }

            // Sovereign - Rout Order will replenish value on kill. Any active Intonation will also replenish value.
            if (origin.isChara)
            {
                origin.Chara.GetCondition<ConOrderRout>()?.Mod(1);
                origin.Chara.GetCondition<ConWeapon>()?.Mod(1);
            }
        }
        return true;
    }
}

