using System;
using Cwl.Helper.Extensions;
namespace SpiritWeapons;

public static class Common
{
    public const int SpiritWeaponEnc = 226000;

    public const int SpiritWeaponHungerMax = 100; // Hunger Goes from Not Hungry (0-49), Hungry (50-79), Starving (80+)
    public const int SpiritWeaponGaugeMax = 100;

    public const int
            SpiritWeaponBondMax =
                    750; // Divide by 5 to get bond level. Increases EXP gain. Bond Goes from: Indifferent (0-24), Respected(25-49), Friendly(50-74), Love(75-99), Absolute Devotion(100+) 

    public const string SpiritWeaponManifestCharaId = "spiritweapon_manifested";

    public static bool IsSpiritWeapon(this Card target)
    {
        return target.HasElement(SpiritWeaponEnc) && (target.IsMeleeWeapon || target.IsRangedWeapon);
    }

    public static bool CanAwakenSpiritWeapon(this Card target)
    {
        return (target.IsMeleeWeapon || target.IsRangedWeapon) && !target.HasElement(SpiritWeaponEnc) && target.rarity != Rarity.Artifact;
    }

    public static void InitializeSpiritWeapon(this Card target, string name, string portrait, string sprite, Chara owner)
    {
        target.SetStr(SpiritWeaponPortrait, portrait);
        target.SetStr(SpiritWeaponSprite, sprite);
        target.SetStr(SpiritWeaponName, name);
        target.SetFlagValue(SpiritWeaponBondTargetFlag, owner.uid);

        // target.SetFlagValue(SpiritWeaponPersonality, EClass.rnd(4));
        target.SetFlagValue(SpiritWeaponPersonality, 0);
        target.c_altName = name;
    }

    public static void IncrementSpiritWeaponGauge(this Card target)
    {
        target.SetFlagValue(SpiritWeaponGauge, Math.Min(target.GetFlagValue(SpiritWeaponGauge) + 1, SpiritWeaponGaugeMax));
    }

    public static void ConsumeSpiritWeaponGauge(this Card target)
    {
        target.SetFlagValue(SpiritWeaponGauge, 0);
    }

    public static void TickSpiritWeaponHunger(this Card target)
    {
        target.SetFlagValue(SpiritWeaponHunger, Math.Min(target.GetFlagValue(SpiritWeaponHunger) + 1, SpiritWeaponHungerMax));
    }

    public static void ReplenishSpiritWeaponHunger(this Card target, int amount)
    {
        target.SetFlagValue(SpiritWeaponHunger, Math.Max(target.GetFlagValue(SpiritWeaponHunger) + amount, 0));
    }

    public static void BondSpiritWeapon(this Card target, int amount = 1)
    {
        target.SetFlagValue(SpiritWeaponBond, Math.Min(target.GetFlagValue(SpiritWeaponBond) + amount, SpiritWeaponBondMax));
    }

    /// <summary>
    ///     Calculates the EXP gain for the Spirit Weapon on consuming a food.
    /// </summary>
    /// <param name="target">The Spirit Weapon being fed.</param>
    /// <param name="food">The weapon being fed to the Spirit Weapon</param>
    /// <returns>
    ///     The Multiplier so that the Spirit will tell you how much they enjoyed it.
    ///     If the weapon is the same category as the Spirit Weapon, 200% Multiplier.
    ///     Every Rarity Level grants an additional 5% exp. So Mythical will give +15%.
    ///     Bond Level Divided by 50 is another exp modifier, up to an additional 30% exp at max bond.
    ///     Blessed State Grants between 25% and -50% "bonus" exp.
    ///     Every enchant level will give another 5%, including negative.
    ///     Maxes out at: 295%.
    ///     If 100-149% Multiplier. It's okay.
    ///     If 150-250% Multiplier. It's good.
    ///     If >250% Multiplier. They love it.
    /// </returns>
    public static float GainSpiritWeaponExperience(this Card target, Card food)
    {
        int currentExp = target.GetFlagValue(SpiritWeaponExperience);
        int currentLvl = target.elements.GetElement(SpiritWeaponEnc).Value;


        int expGain = food.LV;
        float expMod = (target.category.id == food.category.id ? 2 : 1) + food.rarityLv * 0.05F;
        expMod += target.GetFlagValue(SpiritWeaponBond) / 50 / 50F;
        switch (food.blessedState)
        {
            case BlessedState.Blessed:
                expMod += 0.25F;
                break;
            case BlessedState.Cursed:
                expMod -= 0.25F;
                break;
            case BlessedState.Doomed:
                expMod -= 0.5F;
                break;
        }
        expMod += target.encLV * 0.05F;

        expGain = (int)(expGain * expMod);
        long newExp = (long)currentExp + expGain;

        while (true)
        {
            int required = ExpCurve.RequiredExpForLevel(currentLvl);

            if (newExp < required)
            {
                break;
            }

            newExp -= required;
            currentLvl++;
            Msg.Say("spiritweapon_levelup".langGame(target.GetStr(SpiritWeaponName)));

            if (currentLvl is < 0 or > 2000000000)
            {
                currentLvl = int.MaxValue;
                newExp = 0;
                break;
            }
        }

        // Update the levels and exp on the weapon.
        target.elements.SetBase(SpiritWeaponEnc, currentLvl);
        target.SetFlagValue(SpiritWeaponExperience, (int)newExp);

        // Restore Hunger.
        target.ReplenishSpiritWeaponHunger(0 - expGain);

        return expMod;
    }

    public static class ExpCurve
    {
        public static int RequiredExpForLevel(int level)
        {
            const double baseExp = 50.0;
            const double expGrowth = 0.773;

            double exp = baseExp * Math.Pow(level, expGrowth);
            return (int)Math.Round(exp);
        }
    }

    #region Flags

    // Int Flags on the Cards.
    public const string SpiritWeaponBondTargetFlag = "swOwn";
    public const string SpiritWeaponExperience = "swExp";
    public const string SpiritWeaponHunger = "swHG";
    public const string SpiritWeaponBond = "swBd";
    public const string SpiritWeaponGauge = "swGau";
    public const string SpiritWeaponPersonality = "swPer";
    public const string SpiritWeaponUid = "swUid";
    public const string SpiritWeaponBlessingDate = "swBlsDate";

    public const string SpiritWeaponAutoUnleash = "swAU";
    public const string DialogSWCanUseSummon = "swSum";

    public const string DialogSWHunger = "swfHun";
    public const string DialogSWBond = "swfBon";
    public const string DialogSWBondTarget = "swfBonTarg";
    public const string DialogSWVar = "swfVar";
    public const string DialogSWBlessingAvailable = "swfBls";

    // String Flags on the Cards
    public const int SpiritWeaponPortrait = 226001;
    public const int SpiritWeaponSprite = 226002;
    public const int SpiritWeaponName = 226003;

    #endregion

}