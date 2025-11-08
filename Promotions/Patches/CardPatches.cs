using System;
using System.Collections.Generic;
using System.Linq;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements;
using PromotionMod.Elements.Maia;
using PromotionMod.Elements.PromotionAbilities;
using PromotionMod.Stats.Headhunter;
using PromotionMod.Stats.Hexer;
using PromotionMod.Stats.Necromancer;
using PromotionMod.Stats.Sentinel;
using PromotionMod.Stats.Sovereign;
using PromotionMod.Stats.Trickster;
using PromotionMod.Trait;
using PromotionMod.Trait.Artificer;
using PromotionMod.Trait.Trickster;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Card))]
public class CardPatches
{
    [HarmonyPatch(nameof(Card.ModExp), typeof(int), typeof(int))]
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
                int manaValue = chara.mana.value / 5;
                __result += manaValue;
            }

            // Sentinel - Increased PV for Heavy Armor and/or Shield.
            if (chara.Evalue(Constants.FeatSentinel) > 0)
            {
                // Increase PV by 50% if wearing Heavy Armor
                if (chara.GetArmorSkill() == 122)
                {
                    __result = HelperFunctions.SafeMultiplier(__result, 1.5F);
                }

                // Increase PV by 50% if using a Shield
                if (chara.body.GetAttackStyle() == AttackStyle.Shield)
                {
                    __result = HelperFunctions.SafeMultiplier(__result, 1.5F);
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

            // Necromancer - If target is afflicted with ConDeadBeckon, on death can summon a skeleton of their level.
            if (target.HasCondition<ConDeadBeckon>())
            {
                ConDeadBeckon deadBeckon = target.GetCondition<ConDeadBeckon>();
                Chara necromancer = EClass._map.zone.FindChara(deadBeckon.NecromancerUID);

                if (necromancer != null) SpSummonSkeleton.SummonSkeleton(necromancer, target.pos, target.LV, 10, target.LV);
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

            // If a Maiar makes the kill on a Harbinger.
            if (target.trait is TraitHarbinger && origin.Evalue(Constants.FeatMaia) > 0)
            {
                // If the Maia hasn't yet ascended.
                if (origin.Evalue(Constants.FeatMaiaEnlightened) == 0 && origin.Evalue(Constants.FeatMaiaCorrupted) == 0)
                {
                    // Check kills on Harbinger.
                    // If the has already killed a Harbinger, kill the character and skip credit.
                    switch (target.id)
                    {
                        case Constants.CandlebearerCharaId:
                            if (origin.GetFlagValue(Constants.MaiaDarkFateFlag) > 0)
                            {
                                Msg.Say("maiar_forfeit".langGame(origin.NameSimple));
                                origin.Die(null, null, AttackSource.Wrath);
                            }
                            else
                            {
                                Msg.Say("maiar_enlightened".langGame(origin.NameSimple));
                                origin.SetFlagValue(Constants.MaiaLightFateFlag, 1);
                            }
                            break;

                        case Constants.DarklingCharaId:
                            if (origin.GetFlagValue(Constants.MaiaLightFateFlag) > 0)
                            {
                                Msg.Say("maiar_forfeit".langGame(origin.NameSimple));
                                origin.Die(null, null, AttackSource.Wrath);
                            }
                            else
                            {
                                Msg.Say("maiar_corrupted".langGame(origin.NameSimple));
                                EClass.pc.SetFlagValue(Constants.MaiaDarkFateFlag, 1);   
                            }
                            break;
                    }
                }
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

            if (targetConditions.ContainsKey(typeof(ConCorruption)))
            {
                chara.Say("trickster_corruption_nullheal".lang());
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
    
    [HarmonyPatch(nameof(Card.MakeEgg))]
    [HarmonyPrefix]
    internal static bool PromotionMod_PreventMakeEgg_Patch(Card __instance)
    {
        // The Unique Characters in this mod will not drop their genes.
        if (__instance.isChara && 
            __instance.Chara.trait is (TraitHarbinger or TraitSpiritKnight or TraitUniqueSummon or TraitLailah or TraitArtificerGolem))
        {
            return false;
        }
        return true;
    }
    
    [HarmonyPatch(nameof(Card.LevelUp))]
    [HarmonyPostfix]
    internal static void PromotionMod_Maia_LevelUp_Patch(Card __instance)
    {
        // Unascended Maiars will have a warning at level 13, then every level past level 19.
        if (__instance.Evalue(Constants.FeatMaia) > 0 && __instance.Evalue(Constants.FeatMaiaEnlightened) == 0 && __instance.Evalue(Constants.FeatMaiaCorrupted) == 0)
        {
            // If they have not chosen their fate, throw a warning message.
            if (__instance.GetFlagValue(Constants.MaiaLightFateFlag) == 0 && __instance.GetFlagValue(Constants.MaiaDarkFateFlag) == 0)
            {
                if (__instance.LV == 13)
                {
                    Msg.Say("maiar_fate");
                }
                else if (__instance.LV >= 19)
                {
                    Msg.Say("maiar_warning_critical".langGame(__instance.NameSimple));
                }

                // If they have not chosen their fate and have reached level 20 or higher, they will die every level.
                if (__instance.LV >= 20)
                {
                    Msg.Say("maiar_forfeit".langGame(__instance.NameSimple));
                    __instance.Die(null, null, AttackSource.Wrath);
                }
            }
            else
            {
                // If their fate has been chosen, and they reach level 20, ascend them.
                if (__instance.LV >= 20)
                {
                    Msg.Say("maiar_ascension".langGame(__instance.NameSimple));
                    if (__instance.GetFlagValue(Constants.MaiaLightFateFlag) > 0)
                    {
                        // Englightened Ascension
                        __instance.Chara.SetFeat(Constants.FeatMaiaEnlightened, 1, msg: true);
                    }
                    
                    if (__instance.GetFlagValue(Constants.MaiaDarkFateFlag) > 0)
                    {
                        // Corrupted Ascension
                        __instance.Chara.SetFeat(Constants.FeatMaiaCorrupted, 1, msg: true);
                    }
                }
            }
        }
    }
}