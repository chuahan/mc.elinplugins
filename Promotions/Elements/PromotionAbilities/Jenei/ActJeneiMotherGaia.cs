using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiMotherGaia : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJenei) == 0) return false;
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
        int power = GetPower(CC);
        int damage = HelperFunctions.SafeDice("jenei_mothergaia", power);

        // Damage the target on point. Earthquake that point.
        HelperFunctions.ProcSpellDamage(power, damage, CC, TC.Chara, ele: Constants.EleImpact);
        TC.pos.Animate(AnimeID.Quake, true);
        CC.PlaySound("spell_earthquake");
        Shaker.ShakeCam("ball");

        List<Chara> hitTarget = new List<Chara>
        {
            TC.Chara
        };

        _map.ForeachNeighbor(TC.pos, delegate(Point neighbor)
        {
            TweenUtil.Tween(0.8F, null, delegate
            {
                foreach (Chara target in neighbor.Charas.Where(target => !hitTarget.Contains(target) && target.IsHostile(CC)))
                {
                    HelperFunctions.ProcSpellDamage(power, damage / 2, CC, TC.Chara, ele: Constants.EleImpact);
                    hitTarget.Add(target);
                }
                neighbor.Animate(AnimeID.Quake, true);
            });
        });

        return true;
    }
}