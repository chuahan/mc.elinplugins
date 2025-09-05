using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Harbinger;
using PromotionMod.Stats.Hermit;
using PromotionMod.Stats.Ranger;
using PromotionMod.Stats.Sharpshooter;
using PromotionMod.Trait;
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
        // DangerSense - Adventurers
        if (__instance.HasCondition<ConSenseDanger>() && __instance.IsPC && (c.isChara || c.trait is TraitTrap))
        {
            if (c.isChara && c.Chara.IsHostile(__instance) || c.trait is TraitTrap)
            {
                __result = true;
                return;
            }
        }

        // Stealth
        if (c.isChara && c.Chara.HasCondition<ConShadowShroud>())
        {
            int distance = __instance.Dist(c);
            int sightRadius = __instance.GetSightRadius();
            if (distance > 1 || sightRadius < distance)
            {
                __result = false;
                return;
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

    [HarmonyPatch(nameof(Chara.AddCondition), typeof(Condition), typeof(bool))]
    [HarmonyPostfix]
    internal static void ArtificerAutoMedicateAddConditions(Chara __instance, ref Condition __result, Condition c, bool force)
    {
        // Harbinger - Gain damage reduction when nearby enemy afflicted with Harbinger Miasmas.
        if (__result != null && __result is ConHarbingerMiasma)
        {
            foreach (Chara target in EClass.pc.currentZone.map.ListCharasInCircle(__instance.pos, 5f, los:true))
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

        // Artificer - Auto Medicate.
        if (__result != null && __result.Type == ConditionType.Bad)
        {
            foreach (Chara ally in HelperFunctions.GetCharasWithinRadius(__instance.pos, 5F, __instance, true, false))
            {
                if (ally.GetFlagValue(Constants.PromotionFeatFlag) == Constants.FeatHexer)
                {
                    // Allied artificers will automatically try to medicate, 1/4 chance of free medication.
                    if (EClass.rnd(4) == 0)
                    {
                        Msg.Say("artificer_automedicate".lang(ally.NameSimple, __instance.NameSimple));
                        __result.Kill();
                        return;
                    }

                    // If auto medicate fails against PC Artificer, try looking in inventory for the cure.
                    // This technically should happen all the time since Artificer is PC locked.
                    if (ally.IsPC)
                    {
                        // Look in main storage.
                        foreach (Thing medicine in ally.things)
                        {
                            if (medicine.trait.GetHealAction(__instance) != null)
                            {
                                Msg.Say("artificer_automedicate".langGame(ally.NameSimple, __instance.NameSimple));
                                if (EClass.rnd(2) == 0) medicine.ModNum(-1);
                                __result.Kill();
                                return;
                            }
                        }

                        // Look in substorage
                        foreach (Thing container in ally.things.Where(x => x.trait is TraitContainer))
                        {
                            foreach (Thing medicine in ally.things)
                            {
                                if (medicine.trait.GetHealAction(__instance) != null)
                                {
                                    Msg.Say("artificer_automedicate".lang(ally.NameSimple, __instance.NameSimple));
                                    if (EClass.rnd(2) == 0) medicine.ModNum(-1);
                                    __result.Kill();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(nameof(Chara.Die))]
    [HarmonyTranspiler]
    internal static IEnumerable<CodeInstruction> SpawnDoubleLoot(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        var targetMethod = AccessTools.Method(typeof(Card), "SpawnLoot");

        for (int i = 0; i < codes.Count; i++)
        {
            yield return codes[i];

            // Look for call to SpawnLoot
            if (codes[i].opcode == OpCodes.Call && codes[i].operand as MethodInfo == targetMethod)
            {
                // Also yield a second call with the same argument
                yield return codes[i - 1]; // assuming the previous instruction loads the argument
                yield return new CodeInstruction(OpCodes.Call, targetMethod);
            }
        }
    }

    [HarmonyPatch(nameof(Chara._Move))]
    [HarmonyPostfix]
    internal static void MovePostfix(Chara __instance, Point newPoint, Card.MoveType type, ref Card.MoveResult __result)
    {
        // Ranger - If the PC is mounted with Ranger's Canto on, and there is an available target they will make a free shot at the target. Will not work if the weapon needs to be reloaded.
        // Parasites instead use Map.MoveCard or SyncRide.
        if (__instance.IsPC && __instance.HasCondition<StRangerCanto>() && __instance.GetBestRangedWeapon() != null && type == Card.MoveType.Walk && __result == Card.MoveResult.Success && __instance.ride != null)
        {
            Thing rangedWeapon = __instance.GetBestRangedWeapon();
            TraitToolRange traitToolRange = rangedWeapon.trait as TraitToolRange;
            if (!(rangedWeapon.c_ammo <= 0 && traitToolRange.NeedReload))
            {
                foreach (Chara target in HelperFunctions.GetCharasWithinRadius(__instance.pos, (float)rangedWeapon.range, __instance, false, true))
                {
                    ACT.Ranged.Perform(__instance, target);
                    break;
                }
            }
        }
        
        // Sharpshooter - If there is an enemy Sharpshooter in Overwatch Stance within range, they will make a shot at the moving character.
        if (type == Card.MoveType.Walk && __result == Card.MoveResult.Success && __instance.HasCondition<ConUnderFire>())
        {
            foreach (Chara sharpshooter in HelperFunctions.GetCharasWithinRadius(newPoint, 6F, __instance, false, true).Where(sharpshooter => sharpshooter.Evalue(Constants.FeatSharpshooter) > 0 && sharpshooter.HasCondition<StanceOverwatch>()))
            {
                ACT.Ranged.Perform(sharpshooter, __instance);
            }
        }
    }

    [HarmonyPatch(nameof(Chara.SyncRide))]
    [HarmonyPostfix]
    internal static void SyncRidePostfix(Chara __instance, Chara c)
    {
        // Ranger - If the character is currently a symbiote with Ranger's Canto on, and there is an available target they will make a free shot at the target. Will not work if the weapon needs to be reloaded.
        if (c.host == __instance && c.HasCondition<StRangerCanto>() && c.GetBestRangedWeapon() != null)
        {
            Thing rangedWeapon = c.GetBestRangedWeapon();
            TraitToolRange traitToolRange = rangedWeapon.trait as TraitToolRange;
            if (!(rangedWeapon.c_ammo <= 0 && traitToolRange.NeedReload))
            {
                foreach (Chara target in HelperFunctions.GetCharasWithinRadius(c.pos, (float)rangedWeapon.range, c, false, true))
                {
                    ACT.Ranged.Perform(c, target);
                    break;
                }   
            }
        }
    }
}