using System;
using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Stats;
using BardMod.Stats.BardSongConditions;
using HarmonyLib;

namespace BardMod.Patches;

[HarmonyPatch(typeof(Card))]
internal class BardCardPatches : EClass
{
    /*
     * Helper function to handle songs that defy death.
     */
    internal static bool ActivatesDeathDefySongs(Chara chara)
    {
        if (chara != null)
        {
            // Second chance activates if they are above 1/3% HP.
            if (chara.HasCondition<ConLonelyTearsSong>())
            {
                if (chara.hp >= chara.MaxHP / 3)
                {
                    Msg.Say("lonelytears_secondchance".langGame(chara.NameSimple));
                    chara.hp = chara.MaxHP / 100;
                    return true;
                }
            }
        
            // Heavens Fall activates next.
            if (chara.HasCondition<ConHeavensFallSong>())
            {
                ConHeavensFallSong heavensFall = chara.GetCondition<ConHeavensFallSong>();
                if (!heavensFall.ActivatedUndying)
                {
                    Msg.Say("heavensfallsong_defy".langGame(chara.NameSimple));
                    heavensFall.ActivateUndying();
                    return true;
                }
            }
        
            // Homura activates.
            if (chara.HasCondition<ConFarewellFlamesSong>())
            {
                ConFarewellFlamesSong farewellFlames = chara.GetCondition<ConFarewellFlamesSong>();
                Msg.Say("flamesong_defy".langGame(chara.NameSimple));
                chara.hp = 1;
                if (!farewellFlames.BorrowedTime) chara.Talk("flamesong_declare".langGame());
                farewellFlames.BorrowedTime = true;
                //farewellFlames.Mod(-1);
                return true;
            }
        }
        
        return false;
    }
    
    [HarmonyPatch(typeof(Card), nameof(Card.DamageHP), new[] {
        typeof(int),              // dmg
        typeof(int),              // ele
        typeof(int),              // eleP
        typeof(AttackSource),     // attackSource
        typeof(Card),             // origin
        typeof(bool),             // showEffect
        typeof(Thing)             // weapon
    })]
    [HarmonyPrefix]
    internal static bool OnDamageHP(Card __instance, ref int dmg, ref int ele, ref int eleP, AttackSource attackSource, Card origin, bool showEffect, Thing weapon)
    {
        // Prismatic Bridge to Tomorrow
        // If the damage dealer is charged, double a single instance of damage dealt.
        if (origin != null && origin.isChara)
        {
            if (origin.Chara.HasCondition<ConPrismaticBridgeSong>())
            {
                ConPrismaticBridgeSong prismaticBridge = origin.Chara.GetCondition<ConPrismaticBridgeSong>();
                if (prismaticBridge.Stacks > 0)
                {
                    prismaticBridge.Stacks -= 1;
                    dmg = HelperFunctions.SafeMultiplier(dmg,2);
                }
            }
            
            // Serene Fantasia - If the damage dealer has Serene fantasia and ele is not null, damage set to 0.
            if (origin.Chara.HasCondition<ConSereneFantasia>())
            {
                if (ele != 0 && ele != Constants.EleVoid)
                {
                    ConSereneFantasia sereneFantasia = origin.Chara.GetCondition<ConSereneFantasia>();
                    sereneFantasia.Mod(-1);
                    dmg = 0;
                } 
            }
            
            // Reflection of the Abyss - Take 200% more damage and deal 70% damage.
            if (origin.Chara.HasCondition<ConAbyssalReflection>())
            {
                dmg = (int)(dmg * 0.7F);
            }
            
            // Farewell and Gratitude of Flames
            // Increased damage by 5/10/20%
            // Forced Fire Damage Conversion
            if (origin.Chara.HasCondition<ConFarewellFlamesSong>())
            {
                ConFarewellFlamesSong farewellFlames = origin.Chara.GetCondition<ConFarewellFlamesSong>();
                dmg = HelperFunctions.SafeMultiplier(dmg, farewellFlames.AdditionalDamage);
                ele = Constants.EleFire;
                eleP = Math.Max(eleP, farewellFlames.power / 2);
            }
        }
        
        if (__instance.isChara)
        {
            Chara targetChar = __instance.Chara;
            
            // Comatose - Take 50% more damage and remove Comatose.
            if (targetChar.HasCondition<ConComatose>())
            {
                dmg = HelperFunctions.SafeMultiplier(dmg, 1.5F);
                targetChar.RemoveCondition<ConComatose>();
                targetChar.RemoveCondition<ConSleep>();
            }
                
            // Reflection of the Abyss - Take 200% more damage and deal 70% damage.
            if (targetChar.HasCondition<ConAbyssalReflection>())
            {
                dmg = HelperFunctions.SafeMultiplier(dmg, 2);
            }
            
            // Damage negation only affects these damage sources.
            List<AttackSource> damageNegationAttackSources = new List<AttackSource> {
                AttackSource.Melee,
                AttackSource.Range,
                AttackSource.Throw, 
                AttackSource.MagicSword,
                AttackSource.Shockwave,
                AttackSource.None
            };
            
            if (damageNegationAttackSources.Contains(attackSource))
            {
                // Endless Cycle of Plum Blossoms - Target will deflect a part of the damage back at their attack if they are in melee range.
                if (targetChar.HasCondition<ConEndlessBlossomsSong>())
                {
                    ConEndlessBlossomsSong blossomSong = targetChar.GetCondition<ConEndlessBlossomsSong>();
                    int retaliationDamage = (int)(dmg * (blossomSong.CalcRetaliatePercent() / 100));
                    if (origin.isChara && origin.Chara.pos.Distance(targetChar.pos) <= 1)
                    {
                        targetChar.PlayEffect("hit_slash").SetScale(1f);
                        targetChar.Say("blossomsong_retaliate".langGame(targetChar.NameSimple, origin.Chara.NameSimple));
                        origin.Chara.DamageHP(retaliationDamage, AttackSource.Condition, targetChar);
                    }
                    else
                    {
                        targetChar.Say("blossomsong_deflect".langGame(targetChar.NameSimple));
                    }

                    // Reduce the damage taken by the retaliation %.
                    dmg = -retaliationDamage;
                }
                
                // Shimmering Dew before Daybreak - Absorbs % damage prior to DR.
                if (targetChar.HasCondition<ConShimmeringDewSong>())
                {
                    ConShimmeringDewSong shimmeringDew =  targetChar.GetCondition<ConShimmeringDewSong>();
                    int dmgAbsorbed = (int)(dmg * (shimmeringDew.GetDamageAbsorption() / 100));
                    shimmeringDew.DamageAbsorbed = HelperFunctions.SafeAdd(shimmeringDew.DamageAbsorbed, dmgAbsorbed);
                    dmg -= dmgAbsorbed;
                }
                
                // Overguard - Protects flat amount of damage prior to DR.
                if (targetChar.HasCondition<ConOverguard>())
                {
                    ConOverguard overguardCon = targetChar.GetCondition<ConOverguard>();
                    if (overguardCon.value >= dmg)
                    {
                        overguardCon.Mod(-1 * dmg);
                        return false;
                    }

                    dmg -= overguardCon.value;
                    overguardCon.Kill();
                }
            }
            
            // The Heavens Shall Fall - When Undying is activated, all damage is done to healing. 
            if (targetChar.HasCondition<ConHeavensFallSong>())
            {
                ConHeavensFallSong heavensFall = targetChar.GetCondition<ConHeavensFallSong>();
                if (heavensFall.UndyingTurns != 0)
                {
                    targetChar.HealHP(dmg, HealSource.Magic);
                    return false;
                }
            }

            // Death Defiance - Lonely Tears of the Youngest, The Heavens Shall Fall, Farewell and Gratitude of Flames
            // If the damage surpasses the remaining HP of the character, activate.
            // TODO: Should I bother with trying to Mana Body here?
            if (dmg > targetChar.hp) if (ActivatesDeathDefySongs(targetChar)) return false;
        }
        return true;
    }
}