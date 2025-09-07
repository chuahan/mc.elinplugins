using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Trait.ArtificerTools;
using PromotionMod.Trait.Machinist;
namespace PromotionMod.Elements.PromotionAbilities.Artificer;

/// <summary>
///     Artificer Ability
///     Any ally that has one of the consuming tools:
///     - Guns
///     - Bows
///     - Crossbows
///     - Artificer Items
///     Will gain up to 30 Synthesized ammunition that scales power based off of the Artificer's MagDev.
/// </summary>
public class ActSimpleSynthesis : Ability
{
    public override Cost GetCost(Chara c)
    {
        Cost baseCost = base.GetCost(c);
        baseCost.type = CostType.MP;
        return baseCost;
    }

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatArtificer) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.ArtificerId.lang()));
            return false;
        }
        return base.CanPerform();
    }

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

            // Reload Ranged Weapon up to 30 ammo. Creates ether bullets.
            Thing rangedWeapon = chara.GetBestRangedWeapon();
            Thing currentAmmo = chara.FindAmmo(rangedWeapon);
            int currentAmmoCount = currentAmmo.Num;
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
                case TraitToolRangeGun:
                    ammo = ThingGen.Create("bullet", "ether");
                    ammo.SetNum(30 - currentAmmoCount);
                    break;
                case TraitToolRocketLauncher:
                    ammo = ThingGen.Create("rocket", "ether");
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
        return true;
    }
}