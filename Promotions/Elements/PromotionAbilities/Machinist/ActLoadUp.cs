using System;
using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Machinist;

public class ActLoadUp : Ability
{
    private static List<int> BulletMods = new List<int>
    {
        91, // Armor Piercing - Vorpal
        850, // Incendiary - ConvertFire
        66 // Tracer Rounds - Increased Accuracy
    };

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatMachinist) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.MachinistId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, true))
        {
            if (target == CC || target.HasTag(CTAG.machine))
            {
                Thing rangedWeapon = target.GetBestRangedWeapon();
                if (rangedWeapon.trait is not TraitToolRangeGunEnergy && rangedWeapon.trait is not TraitToolRangeGun) continue;
                TraitToolRange rangedTrait = (TraitToolRange)rangedWeapon.trait;
                Thing ammo = new Thing();
                rangedWeapon.c_ammo = rangedTrait.MaxAmmo;
                switch (rangedWeapon.trait)
                {
                    case TraitToolRangeGunEnergy:
                        ammo = ThingGen.Create("bullet_energy", "ether");
                        ammo.SetNum(rangedTrait.MaxAmmo);
                        break;
                    case TraitToolRangeGun:
                        ammo = ThingGen.Create("bullet", "ether");
                        ammo.SetNum(rangedTrait.MaxAmmo);
                        break;
                    default:
                        continue; // Doesn't use ammo.
                }

                int randomEffect = BulletMods.RandomItem();
                ammo.elements.ModBase(randomEffect, 50);

                ammo.SetEncLv(Math.Min(CC.LV, GetPower(CC) / 100));
                rangedWeapon.ammoData = ammo;
            }
        }

        return true;
    }
}