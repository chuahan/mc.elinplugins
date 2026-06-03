using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Trait.Artificer;
namespace PromotionMod.Elements;

/// <summary>
///     Artificer Ability
///     Any ally that has one of the consuming tools:
///     - Guns
///     - Bows
///     - Crossbows
///     - Artificer Items
///     Will gain up to 30 Synthesized ammunition that scales power based off of the Artificer's MagDev.
/// </summary>
public class ActSimpleSynthesis : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatArtificer;
    public override string PromotionString => Constants.ArtificerId;
    public override int AbilityId => Constants.ActSimpleSynthesisId;

    public override bool Perform()
    {
        foreach (Chara chara in pc.party.members)
        {
            // Charge all Artificer Tools by 4.
            List<Thing> artificerTools = chara.things.Where(t => t.trait is TraitArtificerTool).ToList();
            foreach (Thing artificerTool in artificerTools)
            {
                (artificerTool.trait as TraitArtificerTool)?.Recharge(4);
            }
            
            // Reload allies with ammo for their ranged Weapon. Does not work if they have over 30 ammo.
            Thing rangedWeapon = chara.GetBestRangedWeapon();
            if (rangedWeapon != null)
            {
                Thing currentAmmo = chara.FindAmmo(rangedWeapon);
                int currentAmmoCount = currentAmmo?.Num ?? 0;
                if (currentAmmoCount >= 30) continue;
                Thing ammo;
                switch (rangedWeapon.trait)
                {
                    case TraitToolRangeCrossbow:
                        ammo = ThingGen.Create("quarrel", "ether");
                        ammo.SetNum(30 - currentAmmoCount);
                        break;
                    case TraitToolRangeBow:
                        ammo = ThingGen.Create("arrow", "ether");
                        ammo.SetNum(30 - currentAmmoCount);
                        break;
                    case TraitToolRangeGunEnergy:
                        ammo = ThingGen.Create("bullet_energy", "ether");
                        ammo.SetNum(30 - currentAmmoCount);
                        break;
                    case TraitToolRangeGunRocket:
                        ammo = ThingGen.Create("bullet_rocket", "ether");
                        ammo.SetNum(30 - currentAmmoCount);
                        break;
                    case TraitToolRangeGun:
                        ammo = ThingGen.Create("bullet", "ether");
                        ammo.SetNum(30 - currentAmmoCount);
                        break;
                    default:
                        continue; // Doesn't use ammo.
                }

                // Ammo level is based off of your power.
                ammo.SetEncLv(Math.Min(CC.LV, GetPower(CC) / 100));
                ammo.ChangeMaterial("ether");
                ammo.isNPCProperty = true;
                ammo.isGifted = true;
                chara.AddThing(ammo);   
            }
        }
        return true;
    }
}