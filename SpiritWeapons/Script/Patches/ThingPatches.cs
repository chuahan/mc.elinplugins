using System.Linq;
using Cwl.Helper.Extensions;
using HarmonyLib;
using UnityEngine;
namespace SpiritWeapons.Patches;

[HarmonyPatch(typeof(Thing))]
internal class ThingPatches : EClass
{
    // Patch to add the option to mark as selected instrument.
    [HarmonyPatch(typeof(Thing), nameof(Thing.WriteNote))]
    [HarmonyPostfix]
    internal static void SpiritWeapons_WriteNote(Thing __instance, UINote n, IInspect.NoteMode mode, Recipe recipe)
    {
        if (__instance.trait is TraitCoreCrystal)
        {

        }

        if (!__instance.IsSpiritWeapon()) return;

        // Add Information about the Spirit Weapon
        // Add Level (Level of SpiritWeaponEnc enchantment)
        // SpiritWeaponBondTargetFlag
        // SpiritWeaponExperience
        // SpiritWeaponHunger
        // SpiritWeaponBond
        // SpiritWeaponGauge
        // SpiritWeaponPersonality
        string spiritName = __instance.GetStr(Common.SpiritWeaponName);
        int spiritLevel = __instance.Evalue(Common.SpiritWeaponEnc);
        string bondTarget = game.cards.globalCharas.Values.First(gc => gc.uid == __instance.GetFlagValue(Common.SpiritWeaponBondTargetFlag)).Name;
        int spiritBond = __instance.GetFlagValue(Common.SpiritWeaponBondTargetFlag) / 50;
        int spiritGauge = __instance.GetFlagValue(Common.SpiritWeaponGauge);
        int spiritHunger = __instance.GetFlagValue(Common.SpiritWeaponHunger);
        int spiritExpNeeded = Common.ExpCurve.RequiredExpForLevel(spiritLevel) - __instance.GetFlagValue(Common.SpiritWeaponExperience);

        string spiritHungerPhase;
        bool starvingWarning = false;
        switch (spiritHunger)
        {
            case >= 80:
                spiritHungerPhase = "spiritweapon_hunger".langList()[2];
                starvingWarning = true;
                break;
            case >= 50:
                spiritHungerPhase = "spiritweapon_hunger".langList()[1];
                break;
            case < 50:
                spiritHungerPhase = "spiritweapon_hunger".langList()[0];
                break;
        }

        string spiritBondPhase;
        switch (spiritBond)
        {
            case >= 100:
                spiritBondPhase = "spiritweapon_bond".langList()[4];
                break;
            case >= 75:
                spiritBondPhase = "spiritweapon_bond".langList()[3];
                break;
            case >= 50:
                spiritBondPhase = "spiritweapon_bond".langList()[2];
                break;
            case >= 25:
                spiritBondPhase = "spiritweapon_bond".langList()[1];
                break;
            case < 25:
                spiritBondPhase = "spiritweapon_bond".langList()[0];
                break;
        }
        // TODO: If I could get this as a background or something... that would be cooler.
        Sprite weaponSpiritPortrait = Portrait.modPortraits.GetItem(__instance.GetStr(Common.SpiritWeaponPortrait)).GetObject();

        //n.AddHeader("spiritWeapon_Note_NameLevel".lang(spiritName, spiritLevel.ToString()));
        n.AddHeader("spiritWeapon_Note_NameLevelExp".lang(spiritName, spiritLevel.ToString(), spiritExpNeeded.ToString()));
        n.AddImage(weaponSpiritPortrait);
        n.AddText("spiritWeapon_Note_BondTarget".lang(bondTarget, spiritBondPhase));
        if (spiritHunger > 50)
        {
            n.AddText("spiritWeapon_Note_Hunger".lang(spiritName, spiritHungerPhase).TagColor(FontColor.Bad));
            if (starvingWarning)
            {
                n.AddText("spiritWeapon_Note_HungerWarning".lang(spiritName, spiritHungerPhase).TagColor(FontColor.Bad));
            }
        }
        else
        {
            n.AddText("spiritWeapon_Note_Hunger".lang(spiritName, spiritHungerPhase).TagColor(FontColor.Good));
        }
        n.AddText(spiritGauge == 100 ? "spiritWeapon_Note_SpiritGaugeReady".lang().TagColor(FontColor.Good) : "spiritWeapon_Note_SpiritGauge".lang(spiritGauge.ToString()));
    }
}