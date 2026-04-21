using Cwl.Helper.Extensions;
using HarmonyLib;
namespace SpiritWeapons.Patches;

[HarmonyPatch(typeof(AttackProcess))]
public class AttackProcessPatches
{
    [HarmonyPatch(nameof(AttackProcess.CalcHit))]
    [HarmonyPostfix]
    internal static void SpiritWeapons_CalcHitPostfixPatch(AttackProcess __instance, ref bool __result)
    {
        // Pull the attack style.
        // Pull the weapon being used.
        // If the weapon being used is a Spirit Weapon.
        // Spirit Weapon has a chance of saying one of the miss lines.
        if (__instance.TC is { isChara: true })
        {
            Card? spiritWeapon = AttackProcessPatches.EquippedWeaponIsSpiritWeapon(__instance);
            if (spiritWeapon != null)
            {
                if (EClass.rnd(5) == 0)
                {
                    if (__result)
                    {
                        // Funny Hit Message.
                        Msg.Nerun($"spiritweapon_hit_{spiritWeapon.GetFlagValue(Common.SpiritWeaponPersonality)}".langList().RandomItem(), spiritWeapon.GetStr(Common.SpiritWeaponPortrait));
                    }
                    else
                    {
                        // Funny Miss Message.
                        Msg.Nerun($"spiritweapon_miss_{spiritWeapon.GetFlagValue(Common.SpiritWeaponPersonality)}".langList().RandomItem(),
                            spiritWeapon.GetStr(Common.SpiritWeaponPortrait));
                    }
                }
            }
        }
    }

    [HarmonyPatch(nameof(AttackProcess.Perform))]
    [HarmonyPostfix]
    internal static void SpiritWeapons_PerformPostfixPatch(AttackProcess __instance, int count, bool hasHit, ref float dmgMulti, ref bool maxRoll, bool subAttack)
    {
        Chara originChara = __instance.CC;
        Card target = __instance.TC;

        if (target == null || !__instance.TC.isChara || subAttack) return;

        // If the Spirit Weapon is set to use Spirit Unleash.
        Card? spiritWeapon = AttackProcessPatches.EquippedWeaponIsSpiritWeapon(__instance);
        if (spiritWeapon != null && originChara.uid == spiritWeapon.GetFlagValue(Common.SpiritWeaponBondTargetFlag) && spiritWeapon.GetFlagValue(Common.SpiritWeaponAutoUnleash) == 1)
        {
            // See if we can trigger the Spirit Weapon Unleash.
            if (__instance.hit)
            {
                // Check the Spirit Weapon's Hunger - If it's too hungry it won't trigger.
                if (spiritWeapon.GetFlagValue(Common.SpiritWeaponHunger) < 80)
                {
                    // Check the Spirit Weapon's Gauge, if it's at 100 we can activate it.
                    if (spiritWeapon.GetFlagValue(Common.SpiritWeaponGauge) == 100)
                    {
                        spiritWeapon.ConsumeSpiritWeaponGauge();
                        Msg.Nerun($"spiritweapon_unleash_{spiritWeapon.GetFlagValue(Common.SpiritWeaponPersonality)}".langList().RandomItem(),
                            spiritWeapon.GetStr(Common.SpiritWeaponPortrait));
                    }
                    else
                    {
                        // Increment the trigger as it builds up power.
                        spiritWeapon.IncrementSpiritWeaponGauge();
                    }
                    spiritWeapon.BondSpiritWeapon();
                }
            }
        }
    }

    public static Card? EquippedWeaponIsSpiritWeapon(AttackProcess __instance)
    {
        Card? spiritWeapon = null;

        // No real throwing Spirit Weapons.
        if (!__instance.isThrow)
        {
            if (__instance.IsRanged)
            {
                if (__instance.toolRange != null && __instance.toolRange.owner.IsSpiritWeapon())
                {
                    spiritWeapon = __instance.toolRange.owner;
                }
            }
            else
            {
                if (__instance.weapon != null && __instance.weapon.IsSpiritWeapon())
                {
                    spiritWeapon = __instance.weapon;
                }
            }
        }

        return spiritWeapon;
    }

    public static string GetSpiritWeaponUnleashAbility(Card target)
    {
        // Melee: Damage, HP as Damage,
        // Magic: Magic Damage, Magical Critical Hit, MP as Damage
        // Ranged: Critical Damage, Critical Chance, Accuracy

        // Pre Attack Process Abilities.
        // Dagger Spirit - Lethal Strike. Does a critical blow with guaranteed Crit and double damage multiplier.
        // Polearm Spirit - Scales Damage based on Speed.
        // Martial Spirit - Grants a large amount of innate flurry for a short period.

        // Post Attack Process Abilities.
        // Sword Spirit - Sky Slash - Big Blue Great Slash in a beam, basically a Void Beam in that direction. EXCALIBUR
        // Axe Spirit - Great Swing - Large Slash in a 3x3 area in front of you.
        // Blunt Spirit - Grand Swing - Knocks back the enemy, if any enemy is behind them, they take damage like telekinesis.
        // Bow Spirit - Double Bow - Fan of Shots. Fires arrows at all targets in 180 cone.
        // Crossbow Spirit - BFG - Big Green Shot that pierces.
        // Gun Spirit - Bullet Storm - Fully Reloads ammo of the weapon and unloads it at all targets around you. Ends when the gun runs out.
        // Staff Spirit - Magical Chain. The next time you attack, Fire a Magic bolt at the target. Then, if there are any enemies within 3 tiles of them that have not been targeted already, recurse another magic bolt. Repeat up to 3 recurses.
        // Cane Spirit - Magical Overdrive mode. Gain a condition to benefit spellcasters.
        // Scythe Spirit - Veil of Stars - Applies a massive damage debuff on all nearby enemies while healing all allies nearby for 25% of their HP.

        return "";
    }
}