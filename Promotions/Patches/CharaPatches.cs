using System;
using System.Linq;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities;
using PromotionMod.Stats;
using PromotionMod.Stats.Adventurer;
using PromotionMod.Stats.Artificer;
using PromotionMod.Stats.Berserker;
using PromotionMod.Stats.Dancer;
using PromotionMod.Stats.Harbinger;
using PromotionMod.Stats.Headhunter;
using PromotionMod.Stats.Hermit;
using PromotionMod.Stats.Jenei;
using PromotionMod.Stats.Luminary;
using PromotionMod.Stats.Machinist;
using PromotionMod.Stats.Necromancer;
using PromotionMod.Stats.Ranger;
using PromotionMod.Stats.Runeknight;
using PromotionMod.Stats.Sharpshooter;
using PromotionMod.Stats.Sniper;
using PromotionMod.Stats.Sovereign;
using PromotionMod.Stats.WitchHunter;
using PromotionMod.Trait;
using PromotionMod.Trait.Artificer;
using PromotionMod.Trait.Trickster;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Chara))]
internal class CharaPatches : EClass
{
    [HarmonyPatch(nameof(Chara.CanAcceptGift))]
    [HarmonyPrefix]
    internal static bool CanGiftPromotionManual(Chara __instance, ref bool __result, Chara c, Thing t)
    {
        if (t is { trait: TraitPromotionManual })
        {
            if (__instance.things.IsFull())
            {
                return true;
            }
            if (t.c_isImportant)
            {
                return true;
            }

            if (TraitPromotionManual.CanPromote(c))
            {
                __result = true;
                return false;
            }
        }

        return true;
    }

    [HarmonyPatch(nameof(Chara.CanSee))]
    [HarmonyPostfix]
    internal static void CanSeePatching(Chara __instance, ref bool __result, Card c)
    {
        // Hermit - Shadow Shroud
        if (c.isChara && c.Chara.HasCondition<ConShadowShroud>())
        {
            int distance = __instance.Dist(c);
            int sightRadius = __instance.GetSightRadius();
            if (distance > 1 || sightRadius < distance)
            {
                __result = false;
            }

            // Perception + Spotting Skill * 2 and Shadow Shroud reduces detection by 75%.
            int detectionScore = (int)(EClass.rnd(__instance.PER + __instance.Evalue(210) * 2) * 0.25F);
            // Stealth skill * Distance.
            int stealthScore = EClass.rnd(c.Evalue(152) + 5) * (100 + distance * distance * 10) / 100;
            if (stealthScore > detectionScore)
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(nameof(Chara.Refresh))]
    [HarmonyPostfix]
    internal static void RefreshPatch(Chara __instance, bool calledRecursive)
    {
        if (!calledRecursive)
        {
            try
            {
                __instance.visibleWithTelepathy |= (EClass.pc.HasCondition<ConSenseDanger>() && __instance.IsHostile(EClass.pc));
            }
            catch (Exception e)
            {
                // Swallow this cause this can be called without PC being loaded, but Eclass.pc is not null?
            }
        }
    }
    
    [HarmonyPatch(nameof(Chara.AddCondition), typeof(Condition), typeof(bool))]
    [HarmonyPrefix]
    internal static bool AddConditionPrefixPatches(Chara __instance, ref Condition __result, Condition c, bool force)
    {
        // Rune Knight - Warding runes will negate incoming Debuffs.
        ConWardingRune ward = __instance.GetCondition<ConWardingRune>();
        if (ward != null && c.Type == ConditionType.Debuff)
        {
            ward.Mod(-1);
            __result = null;
            return false;
        }

        return true;
    }


    [HarmonyPatch(nameof(Chara.AddCondition), typeof(Condition), typeof(bool))]
    [HarmonyPostfix]
    internal static void AddConditionPostfixPatches(Chara __instance, ref Condition __result, Condition c, bool force)
    {
        // Harbinger - Gain damage reduction when nearby enemy afflicted with Harbinger Miasmas.
        if (__result != null && __result is ConHarbingerMiasma)
        {
            foreach (Chara target in pc.currentZone.map.ListCharasInCircle(__instance.pos, 5f))
            {
                if (target.Evalue(Constants.FeatHarbinger) > 0)
                {
                    if (!target.HasCondition<ConMiasmaArmor>())
                    {
                        target.AddCondition<ConMiasmaArmor>(1);
                    }
                    else
                    {
                        int newStacks = target.GetCondition<ConMiasmaArmor>().GetStacks() + 1;
                        target.AddCondition<ConMiasmaArmor>(newStacks);
                    }
                }
            }
        }

        // Adventurer - Auto Medicate.
        if (__result != null && __result.Type == ConditionType.Bad && __instance.IsPCParty && pc.Evalue(Constants.FeatAdventurer) > 0)
        {
            // Adventurer will automatically try to medicate, 1/4 chance of free medication.
            if (EClass.rnd(4) == 0)
            {
                Msg.Say("adventurer_automedicate".lang(pc.NameSimple, __instance.NameSimple));
                __result.Kill();
                return;
            }

            // If auto medicate fails, try looking in PC inventory for the cure.
            // Look in main storage.
            foreach (Thing medicine in pc.things)
            {
                if (medicine.trait.GetHealAction(__instance) != null)
                {
                    Msg.Say("adventurer_automedicate".langGame(pc.NameSimple, __instance.NameSimple));
                    if (EClass.rnd(2) == 0) medicine.ModNum(-1);
                    __result.Kill();
                    return;
                }
            }

            // Look in substorage
            foreach (Thing container in pc.things.Where(x => x.trait is TraitContainer))
            {
                foreach (Thing medicine in pc.things)
                {
                    if (medicine.trait.GetHealAction(__instance) != null)
                    {
                        Msg.Say("adventurer_automedicate".langGame(pc.NameSimple, __instance.NameSimple));
                        if (EClass.rnd(2) == 0) medicine.ModNum(-1);
                        __result.Kill();
                        return;
                    }
                }
            }
        }
    }

    [HarmonyPatch(nameof(Chara.Die))]
    [HarmonyPrefix]
    internal static bool Promotion_CharaDie_PrefixPatch(Chara __instance, Card origin)
    {
        if (__instance.isChara)
        {
            // Adventurer - Double Loot.
            if (!__instance.IsPC && !(__instance.IsPCParty || __instance.IsPCFactionMinion) && __instance.IsInActiveZone && EClass.pc.Evalue(Constants.FeatAdventurer) > 0)
            {
                __instance.SpawnLoot(origin);
            }
            
            Chara target = __instance.Chara;

            if (origin is { Chara: not null, isChara: true })
            {
                Chara originChara = origin.Chara;
                // Berserker - Heal on Kill
                if (originChara.Evalue(Constants.FeatBerserker) > 0)
                {
                    int healAmount = (int)(originChara.MaxHP * .25F);
                    origin.Say("berserker_revel".langGame(originChara.NameSimple));
                    originChara.HealHP(healAmount);
                }

                // Headhunter - Gain Headhunter stacks on Kill.
                if (originChara.Evalue(Constants.FeatHeadhunter) > 0)
                {
                    if (!originChara.HasCondition<ConHeadhunter>())
                    {
                        originChara.AddCondition<ConHeadhunter>(1);
                    }
                    else
                    {
                        int newStacks = originChara.GetCondition<ConHeadhunter>().power + 1;
                        originChara.AddCondition<ConHeadhunter>(newStacks);
                    }
                }
                
                // Headhunter - Steal a buff on kill.
                if (origin.isChara && originChara.Evalue(Constants.FeatHeadhunter) > 0)
                {
                    Condition? buff = target.conditions.FirstOrDefault(x => x.Type == ConditionType.Buff);
                    if (buff != null)
                    {
                        originChara.AddCondition(buff.source.alias, buff.power, true);
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
                    originChara.GetCondition<ConOrderRout>()?.Mod(1);
                    originChara.GetCondition<ConWeapon>()?.Mod(1);
                }

                // Trickster - Phantom Trickster Ids will inflict one of the random Trickster debuffs on their killer.
                if (origin.isChara && originChara.IsHostile(target) && target.id == Constants.PhantomTricksterCharaId)
                {
                    string randomCondition = TraitTricksterArcaneTrap.TricksterDebuffs.RandomItem();
                    Condition tricksterCondition = Condition.Create(randomCondition, target.LV);
                    originChara.AddCondition(tricksterCondition, true);
                    origin.PlayEffect("curse");
                }

                // If a Maiar makes the kill on a Harbinger.
                if (target.trait is TraitHarbinger && originChara.Evalue(Constants.FeatMaia) > 0)
                {
                    // If the Maia hasn't yet ascended.
                    if (originChara.Evalue(Constants.FeatMaiaEnlightened) == 0 && originChara.Evalue(Constants.FeatMaiaCorrupted) == 0)
                    {
                        // Check kills on Harbinger.
                        // If the has already killed a Harbinger, kill the character and skip credit.
                        switch (target.id)
                        {
                            case Constants.CandlebearerCharaId:
                                if (originChara.GetFlagValue(Constants.MaiaDarkFateFlag) > 0)
                                {
                                    Msg.Say("maiar_forfeit".langGame(originChara.NameSimple));
                                    originChara.Die(null, null, AttackSource.Wrath);
                                }
                                else
                                {
                                    Msg.Say("maiar_enlightened".langGame(originChara.NameSimple));
                                    originChara.SetFlagValue(Constants.MaiaLightFateFlag);
                                }
                                break;

                            case Constants.DarklingCharaId:
                                if (originChara.GetFlagValue(Constants.MaiaLightFateFlag) > 0)
                                {
                                    Msg.Say("maiar_forfeit".langGame(originChara.NameSimple));
                                    originChara.Die(null, null, AttackSource.Wrath);
                                }
                                else
                                {
                                    Msg.Say("maiar_corrupted".langGame(originChara.NameSimple));
                                    originChara.SetFlagValue(Constants.MaiaDarkFateFlag);
                                }
                                break;
                        }
                    }
                }
            }
        }
        return true;
    }

    [HarmonyPatch(nameof(Chara._Move))]
    [HarmonyPrefix]
    internal static bool MovePrefix(Chara __instance, Point newPoint, Card.MoveType type, ref Card.MoveResult __result)
    {
        if (__instance.HasCondition<StanceHeavyarms>())
        {
            __result = Card.MoveResult.Fail;
            __instance.Say("machinist_heavyarms_nomove".langGame(__instance.NameSimple));
            return false;
        }

        return true;
    }

    [HarmonyPatch(nameof(Chara._Move))]
    [HarmonyPostfix]
    internal static void MovePostfix(Chara __instance, Point newPoint, Card.MoveType type, ref Card.MoveResult __result)
    {
        // Ranger - If the PC is mounted with Ranger's Canto on, and there is an available target they will make a free shot at the target. Will not work if the weapon needs to be reloaded.
        // Parasites instead use Map.MoveCard or SyncRide.
        if (!_zone.IsRegion &&
            __instance.IsPC &&
            __instance.HasCondition<StanceRangerCanto>() &&
            __instance.GetBestRangedWeapon() != null &&
            type == Card.MoveType.Walk &&
            __result == Card.MoveResult.Success &&
            __instance.ride != null)
        {
            Thing rangedWeapon = __instance.GetBestRangedWeapon();
            TraitToolRange traitToolRange = rangedWeapon.trait as TraitToolRange;
            __instance.ranged = rangedWeapon;
            if (!(rangedWeapon.c_ammo <= 0 && traitToolRange.NeedReload))
            {
                foreach (Chara target in HelperFunctions.GetCharasWithinRadius(__instance.pos, rangedWeapon.range, __instance, false, true))
                {
                    ACT.Ranged.Perform(__instance, target);
                }
            }
        }

        // Sharpshooter - If there is an enemy Sharpshooter in Overwatch Stance within range, they will make a shot at the moving character.
        if (type == Card.MoveType.Walk && __result == Card.MoveResult.Success && __instance.HasCondition<ConUnderFire>())
        {
            foreach (Chara sharpshooter in HelperFunctions.GetCharasWithinRadius(newPoint, 6F, __instance, false, true)
                             .Where(sharpshooter => sharpshooter.Evalue(Constants.FeatSharpshooter) > 0 && sharpshooter.HasCondition<StanceOverwatch>()))
            {
                Thing rangedWeapon = __instance.GetBestRangedWeapon();
                __instance.ranged = rangedWeapon;
                ACT.Ranged.Perform(sharpshooter, __instance);
            }
        }
    }

    [HarmonyPatch(nameof(Chara.SyncRide), typeof(Chara))]
    [HarmonyPostfix]
    internal static void SyncRidePostfix(Chara __instance, Chara c)
    {
        // Ranger - If the character is currently a symbiote with Ranger's Canto on, and there is an available target they will make a free shot at the target. Will not work if the weapon needs to be reloaded.
        if (!_zone.IsRegion &&
            c.host == __instance &&
            c.HasCondition<StanceRangerCanto>() &&
            c.GetBestRangedWeapon() != null)
        {
            Thing rangedWeapon = c.GetBestRangedWeapon();
            TraitToolRange traitToolRange = rangedWeapon.trait as TraitToolRange;
            if (!(rangedWeapon.c_ammo <= 0 && traitToolRange.NeedReload))
            {
                foreach (Chara target in HelperFunctions.GetCharasWithinRadius(c.pos, rangedWeapon.range, c, false, true))
                {
                    ACT.Ranged.Perform(c, target);
                    break;
                }
            }
        }
    }

    [HarmonyPatch(nameof(Chara.CalcCastingChance))]
    [HarmonyPostfix]
    internal static void CalcCastingChance(Chara __instance, Element e, int num, ref int __result)
    {
        // Dancer - Infatuation reduces cast chance by 25%.
        if (__instance.Chara.HasCondition<ConInfatuation>())
        {
            __result = (int)(__result * 0.75F);
        }

        // WitchHunter - Null Presence prevents casting.
        if (__instance.Chara.HasCondition<ConNullPresence>())
        {
            __result = 0;
        }
    }

    [HarmonyPatch(nameof(Chara.TryNullifyCurse))]
    [HarmonyPrefix]
    internal static bool TryNullifyCurse(Chara __instance, ref bool __result)
    {
        // Corrupted Maias are immune to curses.
        if (__instance.Evalue(Constants.FeatMaiaCorrupted) > 0)
        {
            __result = true;
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(Chara.MakeGene))]
    [HarmonyPrefix]
    internal static bool MakeGene(Chara __instance)
    {
        // The Unique Characters in this mod will not drop their genes.
        if (__instance.trait is TraitHarbinger or TraitSpiritKnight or TraitUniqueSummon or TraitLailah or TraitArtificerGolem)
        {
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(Chara.Tick))]
    [HarmonyPrefix]
    internal static bool CharaTick_Patches(Chara __instance)
    {
        int promotionId = __instance.GetFlagValue(Constants.PromotionFeatFlag);
        // Class-specific passives:
        // Berserker
        // Sniper
        switch (promotionId)
        {
            case Constants.FeatBerserker:
                if (__instance.HasCondition<ConBerserker>() == false)
                {
                    __instance.AddCondition<ConBerserker>();
                }
                break;
            case Constants.FeatSniper:
                // If the sniper doesn't have the condition and there are no enemies with 3 tiles.
                if (__instance.HasCondition<ConNoDistractions>() == false && HelperFunctions.GetCharasWithinRadius(__instance.pos, 3, __instance, false, true).Count == 0)
                {
                    __instance.AddCondition<ConNoDistractions>();
                }
                break;
        }

        // Harpy Golem will attempt to add extra FOV Condition to the player.
        if (__instance is { IsPCParty: true, IsAliveInCurrentZone: true } && __instance.Evalue(Constants.FeatHarpyGolemVision) > 0 && EClass.pc.IsAliveInCurrentZone)
        {
            Condition? visionBuff = EClass.pc.GetCondition<ConAerialVision>() ?? EClass.pc.AddCondition<ConAerialVision>();
            if (visionBuff is { value: <= 1 }) visionBuff?.Mod(5);
        }

        // Siren Golems gain Adaptive Mobility when floating
        // Siren Golems gain Liquid Cooling when Wet
        if (__instance.IsAliveInCurrentZone)
        {
            if (__instance.Evalue(Constants.FeatSirenGolemSpeed) > 0 && (EClass._zone.IsUnderwater || __instance.Chara.isFloating))
            {
                Condition? movementBuff = EClass.pc.GetCondition<ConAdaptiveMobility>() ?? EClass.pc.AddCondition<ConAdaptiveMobility>();
                if (movementBuff is { value: <= 1 }) movementBuff?.Mod(5);
            }

            if (__instance.HasCondition<ConWet>() && __instance.Evalue(Constants.FeatSirenGolemMagic) > 0)
            {
                Condition? castingBuff = EClass.pc.GetCondition<ConLiquidCooling>() ?? EClass.pc.AddCondition<ConLiquidCooling>();
                if (castingBuff is { value: <= 1 }) castingBuff?.Mod(5);
            }
        }
        return true;
    }

    [HarmonyPatch(nameof(Chara.TickConditions))]
    [HarmonyPrefix]
    internal static bool CharaTickConditions_Patches(Chara __instance)
    {
        // Two conditions will hasten cooldowns by 1 extra turn.
        if (__instance.HasCondition<StanceHeavyarms>())
        {
            if (__instance._cooldowns != null)
            {
                __instance.TickCooldown();
            }
        }

        if (__instance.HasCondition<ConAcceleration>())
        {
            if (__instance._cooldowns != null)
            {
                __instance.TickCooldown();
            }
        }
        return true;
    }
}