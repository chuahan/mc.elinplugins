using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.Maia;

/// <summary>
/// Vengeance deals dark spell damage to all enemies within FOV.
/// </summary>
public class ActCorruptedVengeance : Ability
{
    public override bool CanPerform()
    {
        // Ability is only usable by Corrupted Maia.
        if (CC.Evalue(Constants.FeatMaia) == 0 || (CC.Evalue(Constants.FeatMaiaCorrupted) == 0))
        {
            return false;
        }
        
        // Can't be used in world map.
        if (_zone.IsRegion)
        {
            return false;
        }

        return base.CanPerform();
    }

    public override bool Perform()
    {
        List<Point> targets = new List<Point>();
        foreach (Point p in CC.fov.ListPoints())
        {
            foreach(Chara c in p.Charas)
            {
                if (c.IsHostile(CC))
                {
                    targets.Add(p);
                    break;
                }
            }
        }
        if (targets.Count == 0)
        {
            return false;
        }
        
        int power = this.GetPower(CC);
        ActEffect.DamageEle(CC, EffectId.Ball, power, Element.Create(Constants.EleDarkness, power / 10), targets, new ActRef()
        {
            act = this,
        });
        return true;
    }
}