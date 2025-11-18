using System;
using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Machinist;

public class ActLoadUp : Ability
{
    private static List<List<int>> BulletMods = new List<List<int>>
    {
        new List<int>
        {
            91,
            606
        }, // Armor Piercing - Vorpal and Drill
        new List<int>
        {
            850,
            607
        }, // Incendiary - ConvertFire and Scatter
        new List<int>
        {
            66,
            620
        } // Tracer Rounds - Accuracy and Chaser
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

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
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
                    case TraitToolRangeGunRocket:
                        ammo = ThingGen.Create("bullet_rocket", "ether");
                        ammo.SetNum(rangedTrait.MaxAmmo);
                        break;
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

                List<int> ammoType = BulletMods.RandomItem();
                ammo.elements.ModBase(ammoType[0], Math.Max(GetPower(CC) / 100, 25));
                ammo.elements.ModBase(ammoType[1], 50);

                ammo.SetEncLv(Math.Min(CC.LV, GetPower(CC) / 100));
                rangedWeapon.ammoData = ammo;
            }
        }

        return true;
    }
}