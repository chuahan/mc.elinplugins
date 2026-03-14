using System;
using System.Linq;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities;
using PromotionMod.Stats.Adventurer;
using PromotionMod.Stats.Artificer;
using PromotionMod.Stats.Berserker;
using PromotionMod.Stats.Dancer;
using PromotionMod.Stats.Harbinger;
using PromotionMod.Stats.Headhunter;
using PromotionMod.Stats.Hermit;
using PromotionMod.Stats.Machinist;
using PromotionMod.Stats.Necromancer;
using PromotionMod.Stats.QuestStats;
using PromotionMod.Stats.Ranger;
using PromotionMod.Stats.Runeknight;
using PromotionMod.Stats.Sharpshooter;
using PromotionMod.Stats.Sniper;
using PromotionMod.Stats.Sovereign;
using PromotionMod.Stats.WitchHunter;
using PromotionMod.Trait;
using PromotionMod.Trait.Artificer;
using PromotionMod.Trait.Characters;
using PromotionMod.Trait.Trickster;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Chara))]
internal class CharaPatches : EClass
{
    /*
    [HarmonyPatch(nameof(Chara.UseAbility), typeof(string), typeof(Card), typeof(Point), typeof(bool))]
    [HarmonyPrefix]
    internal static bool DebuggingPatchUseAbility(Chara __instance, string idAct, Card tc, Point pos, bool pt)
    {
        Msg.Say(__instance.NameSimple);
        Msg.Say(idAct);
        Msg.Say(tc.Name);
        Msg.Say(pt.ToString());
        Act elementAct = __instance.elements.GetElement(idAct)?.act;
        Act createAct = ACT.Create(idAct);
        return true;
    }
    */
    
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

            if (TraitPromotionManual.CanPromote(__instance))
            {
                __result = true;
                return false;
            }
        }
        
        if (t is { trait: TraitDemotionManual })
        {
            if (__instance.things.IsFull())
            {
                return true;
            }
            if (t.c_isImportant)
            {
                return true;
            }

            if (__instance.GetFlagValue(Constants.PromotionFeatFlag) != 0)
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
    [HarmonyPostfix]
    internal static void AddConditionPostfixPatches(Chara __instance, ref Condition __result, Condition c, bool force)
    {
        // Harbinger - Gain damage reduction when nearby enemy afflicted with Harbinger Miasmas.
        if (__result != null && __result is ConHarbingerMiasma)
        {
            foreach (Chara target in pc.currentZone.map.ListCharasInCircle(__instance.pos, 5f))
            {
                if (target.MatchesPromotion(Constants.FeatHarbinger))
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
        if (__result is { Type: ConditionType.Bad } &&
            __instance.IsPCParty &&
            pc.MatchesPromotion(Constants.FeatAdventurer))
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
        
        // Rune Knight - Warding runes will negate incoming Debuffs.
        ConWardingRune ward = __instance.GetCondition<ConWardingRune>();
        if (ward != null && c.Type == ConditionType.Debuff)
        {
            ward.Mod(-1);
            c.Kill();
        }
    }

    [HarmonyPatch(nameof(Chara.Die))]
    [HarmonyPrefix]
    internal static bool Promotion_CharaDie_PrefixPatch(Chara __instance, Card origin)
    {
        if (__instance.isChara)
        {
            // Adventurer - Double Loot.
            if (!__instance.IsPC && !(__instance.IsPCParty || __instance.IsPCFactionMinion) && __instance.IsInActiveZone && EClass.pc.MatchesPromotion(Constants.FeatAdventurer))
            {
                __instance.SpawnLoot(origin);
            }
            
            Chara target = __instance.Chara;

            if (origin is { Chara: not null, isChara: true })
            {
                // Heal on kill does not get impacted by reduced healing effects.
                Chara originChara = origin.Chara;
                // Berserker - Heal on Kill
                if (originChara.MatchesPromotion(Constants.FeatBerserker))
                {
                    int healAmount = (int)(originChara.MaxHP * .25F);
                    origin.Say("berserker_revel".langGame(originChara.NameSimple));
                    HealHpDirect(originChara, healAmount);
                }
                
                // Dread Knight - Heal on Kill
                if (originChara.MatchesPromotion(Constants.FeatDreadKnight))
                {
                    int healAmount = (int)(originChara.MaxHP * .1F);
                    origin.Say("dreadknight_lifetaker".langGame(originChara.NameSimple));
                    HealHpDirect(originChara, healAmount);
                }

                // Headhunter - Gain Headhunter stacks on Kill.
                if (originChara.MatchesPromotion(Constants.FeatHeadhunter))
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
                if (origin.isChara && originChara.MatchesPromotion(Constants.FeatHeadhunter))
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
                // Heal 2% life/stamina/mana on kill.
                if (origin.isChara)
                {
                    originChara.GetCondition<ConOrderRout>()?.Mod(1);
                    originChara.GetCondition<ConWeapon>()?.Mod(1);
                    int lifeHeal = (int)(originChara.MaxHP * .02F);
                    int manaHeal = (int)(originChara.mana.max * .02F);
                    int staminaHeal = (int)(originChara.stamina.max * .02F);
                    originChara.HealHP(lifeHeal, HealSource.HOT);
                    originChara.mana.Mod(manaHeal);
                    originChara.stamina.Mod(staminaHeal);
                    
                }

                // Trickster - Phantom Trickster Ids will inflict one of the random Trickster debuffs on their killer.
                if (origin.isChara && originChara.IsHostile(target) && target.id == Constants.PhantomTricksterCharaId)
                {
                    string randomCondition = TraitTricksterArcaneTrap.TricksterDebuffs.RandomItem();
                    Condition tricksterCondition = Condition.Create(randomCondition, target.LV);
                    originChara.AddCondition(tricksterCondition, true);
                    origin.PlayEffect("curse");
                }
            }
        }
        return true;
    }

    internal static void HealHpDirect(Chara c, long amount)
    {
        // Cap Healing to big number.
        if (amount > 100000000L) amount = 100000000L;
        c.hp += (int) amount;
        if (c.hp > c.MaxHP) c.hp = c.MaxHP;
        c.PlaySound("heal");
        c.PlayEffect("heal");
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
        // For Quests:
        //if (EClass.pc.currentZone == QuestManager.)
        
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
            foreach (Chara sharpshooter in HelperFunctions.GetCharasWithinRadius(newPoint, 10F, __instance, false, true)
                             .Where(sharpshooter => sharpshooter.MatchesPromotion(Constants.FeatSharpshooter) && sharpshooter.HasCondition<StanceOverwatch>()))
            {
                Thing rangedWeapon = sharpshooter.GetBestRangedWeapon();
                if (rangedWeapon == null) continue;
                Msg.Say("sharpshooter_overwatch".langGame(sharpshooter.NameSimple, __instance.NameSimple));
                sharpshooter.ranged = rangedWeapon;
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
    
    [HarmonyPatch(nameof(Chara.MakeGene))]
    [HarmonyPrefix]
    internal static bool MakeGene(Chara __instance)
    {
        // The Unique Summons and Golems will not drop their Genes.
        if (__instance.trait is TraitUniqueSummon or TraitArtificerGolem)
        {
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(Chara.TickConditions))]
    [HarmonyPrefix]
    internal static bool CharaTickConditions_Patches(Chara __instance)
    {
        // This is used by the original function to regulate the rate of these calculations.
        int turnMod = (__instance.turn + 1) % 50;
        
        // Create a Lookup Table for reducing looping.
        ILookup<Type, Condition> activeConditions = __instance.conditions.ToLookup(c => c.GetType());
        
        switch (turnMod)
        {
            case 1:
            {
                // Update class-specific passives every turn.
                // Berserker
                // Sniper
                int promotionId = __instance.GetFlagValue(Constants.PromotionFeatFlag);
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
            
                // Alraune - If Daylight AND wet, mod hunger 2 (in additional to the original 1, making them digest 3x as fast.
                if (__instance.HasElement(Constants.FeatAlraune) &&
                    __instance.pos.IsSunLit &&
                    activeConditions.Contains(typeof(ConWet)))
                {
                    __instance.hunger.Mod(2);
                }
                break;
            }
            case 5:
            {
                // Alraunes - If Daylight, self-cast Natures Embrace every 30 turns.
                if (__instance.HasElement(Constants.FeatAlraune) &&
                    __instance.pos.IsSunLit &&
                    !__instance.HasCondition<ConHOT>())
                {
                    Element element = __instance.elements.GetElement(8450);
                    if (element != null)
                    {
                        __instance.elements.ModExp(element.id, 20f);
                        __instance.AddCondition<ConHOT>(element.GetPower(__instance));   
                    }
                }
            
                // Harpy Golem will attempt to add extra FOV Condition to the player.
                if (__instance is { IsPCParty: true, IsAliveInCurrentZone: true } &&
                    __instance.HasElement(Constants.FeatHarpyGolemVisionId) &&
                    EClass.pc.IsAliveInCurrentZone)
                {
                    Condition? visionBuff = EClass.pc.GetCondition<ConAerialVision>() ?? EClass.pc.AddCondition<ConAerialVision>();
                    if (visionBuff is { value: <= 1 }) visionBuff?.Mod(5);
                }
            
                // Siren Golems gain Adaptive Mobility when floating
                // Siren Golems gain Liquid Cooling when Wet
                if (__instance.IsAliveInCurrentZone)
                {
                    if (__instance.HasElement(Constants.FeatSirenGolemSpeedId) &&
                        (EClass._zone.IsUnderwater || __instance.Chara.isFloating))
                    {
                        if (activeConditions.Contains(typeof(ConAdaptiveMobility)))
                        {
                            Condition refreshCon = activeConditions[typeof(ConAdaptiveMobility)].Single();
                            if (refreshCon is { value: <= 1 }) refreshCon?.Mod(5);
                        } else
                        {
                            __instance.AddCondition<ConAdaptiveMobility>();
                        }
                    }

                    if (activeConditions.Contains(typeof(ConWet)) && __instance.HasElement(Constants.FeatSirenGolemMagicId))
                    {
                        if (activeConditions.Contains(typeof(ConLiquidCooling)))
                        {
                            Condition refreshCon = activeConditions[typeof(ConLiquidCooling)].Single();
                            if (refreshCon is { value: <= 1 }) refreshCon?.Mod(5);
                        } else
                        {
                            __instance.AddCondition<ConLiquidCooling>();
                        }
                    }
                }
                break;
            }
        }
        
        return true;
    }
    
    [HarmonyPatch(nameof(Chara.GetName))]
    [HarmonyPostfix]
    internal static void CharaName_PrefixSkill_Patch(Chara __instance, ref string __result, NameStyle style, int num)
    {
        // Do I want to make promoted enemies also have a title?
        if (!__instance.IsPCFactionOrMinion && __instance.GetFlagValue(Constants.IsEliteEnemyFlag) != 0)
        {
            // If an enemy is an advanced enemy through spawn. Grant them a prefix so players know what to expect.
            int advancedCombatSkill = __instance.GetFlagValue(Constants.AdvancedCombatSkillFlag);
            if (advancedCombatSkill != 0) __result = $"{EClass.sources.elements.map[advancedCombatSkill].alias}_prefix".lang(__result);
        }
    }
}