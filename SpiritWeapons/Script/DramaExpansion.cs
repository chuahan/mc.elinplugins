using System.Collections.Generic;
using System.Linq;
using Cwl.API.Drama;
using Cwl.Helper.Extensions;
namespace SpiritWeapons;

public class DramaExpansion : DramaOutcome
{
    private static bool SpiritWeaponStateCheck(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara actor);

        // Get the Spirit Weapon too.
        Thing spiritWeapon = DramaExpansion.FindSpiritWeaponThing(EClass.pc, actor);

        // Reset all Flags.
        actor.SetFlagValue(Common.DialogSWHunger, 0);
        actor.SetFlagValue(Common.DialogSWBond, 0);
        actor.SetFlagValue(Common.DialogSWBondTarget, 0);
        actor.SetFlagValue(Common.DialogSWVar, EClass.rnd(4));
        actor.SetFlagValue(Common.DialogSWBlessingAvailable, 0);

        // Set Hunger Flag.
        int spiritHunger = spiritWeapon.GetFlagValue(Common.SpiritWeaponHunger);
        switch (spiritHunger)
        {
            case >= 80:
                actor.SetFlagValue(Common.DialogSWHunger, 2);
                break;
            case >= 50:
                actor.SetFlagValue(Common.DialogSWHunger);
                break;
        }

        // Set Bond Flag.
        int spiritBond = spiritWeapon.GetFlagValue(Common.SpiritWeaponBondTargetFlag) / 50;
        switch (spiritBond)
        {
            case >= 100:
                actor.SetFlagValue(Common.DialogSWBond, 4);
                break;
            case >= 75:
                actor.SetFlagValue(Common.DialogSWBond, 3);
                break;
            case >= 50:
                actor.SetFlagValue(Common.DialogSWBond, 2);
                break;
            case >= 25:
                actor.SetFlagValue(Common.DialogSWBond);
                break;
        }

        // Check if Blessing is Available.
        if (spiritWeapon.GetFlagValue(Common.SpiritWeaponBlessingDate) <= EClass.world.date.day && spiritBond > 50)
        {
            spiritWeapon.SetFlagValue(Common.SpiritWeaponBlessingDate, 0);
            actor.SetFlagValue(Common.DialogSWBlessingAvailable);
        }

        // Whether the player is talking to a weapon bonded to them or not. This is mostly in case if other NPCs can use Spirit Weapons.
        if (EClass.pc.uid == spiritWeapon.GetFlagValue(Common.SpiritWeaponBondTargetFlag)) actor.SetFlagValue(Common.DialogSWBondTarget);

        // Set the Gauge Usage Flag. Defaults to 0/Auto Use Unleash. If set to 1, will not use Spirit Unleash.
        // Can only be set if Bond > 100 (Spirit Summon Usable)
        actor.SetFlagValue(Common.SpiritWeaponAutoUnleash, spiritWeapon.GetFlagValue(Common.SpiritWeaponAutoUnleash));
        actor.SetFlagValue(Common.DialogSWCanUseSummon, spiritBond >= 100 ? 1 : 0);

        return true;
    }

    private static bool FeedSpiritWeapon(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara actor);

        // Get the weapon to be fed.
        Thing spiritWeapon = DramaExpansion.FindSpiritWeaponThing(EClass.pc, actor);
        LayerDragGrid.Create(new InvOwnerFeedSpiritWeapon(spiritWeapon));
        return true;
    }

    private static bool SpiritWeaponBlessing(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara actor);

        // Add a blessing to the PC.
        DramaExpansion.ApplySpiritBlessing(actor.c_altName, EClass.pc);

        // Add a cooldown till the next day.
        Thing spiritWeapon = DramaExpansion.FindSpiritWeaponThing(EClass.pc, actor);
        spiritWeapon.SetFlagValue(Common.SpiritWeaponBlessingDate, EClass.world.date.day + 1);
        return true;
    }

    public static void ApplySpiritBlessing(string spiritWeaponName, Chara cc)
    {
        int blessing = EClass.rnd(5);
        switch (blessing)
        {
            case 0:
                // Restore Stamina
                Msg.Say("spiritweapon_staminablessing".langGame(spiritWeaponName));
                cc.stamina.Mod(cc.stamina.max);
                break;
            case 1:
                // Restore Health
                Msg.Say("spiritweapon_healthblessing".langGame(spiritWeaponName));
                cc.HealHP(cc.MaxHP, HealSource.Item);
                break;
            case 2:
                // Restore Mana
                Msg.Say("spiritweapon_manablessing".langGame(spiritWeaponName));
                cc.mana.Mod(cc.mana.max);
                break;
            case 3:
                // Blesses a random item with +1.
                foreach (Thing thing in cc.things.Where(thing => thing.isEquipped && thing.encLV < 5))
                {
                    thing.encLV++;
                    Msg.Say("spiritweapon_equipmentblessing".langGame(spiritWeaponName, thing.Name));
                    break;
                }
                break;
            case 4:
                // Repairs a damaged item up to 0 if possible.
                foreach (Thing thing in cc.things.Where(thing => thing.isEquipped && thing.encLV < 0))
                {
                    thing.encLV = 0;
                    Msg.Say("spiritweapon_repairblessing".langGame(spiritWeaponName, thing.Name));
                    break;
                }
                break;
        }
    }

    public static Thing FindSpiritWeaponThing(Chara c, Chara spiritWeapon)
    {
        int uid = spiritWeapon.GetFlagValue(Common.SpiritWeaponUid);
        return c.things.Find(uid);
    }

    private static bool BakeSpiritUnleashSettings(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara actor);
        // Find the SpiritWeapon and set whatever is currently set on the flag to the spirit weapon.
        Thing spiritWeapon = DramaExpansion.FindSpiritWeaponThing(EClass.pc, actor);
        spiritWeapon.SetFlagValue(Common.SpiritWeaponAutoUnleash, actor.GetFlagValue(Common.SpiritWeaponAutoUnleash));
        return true;
    }
}