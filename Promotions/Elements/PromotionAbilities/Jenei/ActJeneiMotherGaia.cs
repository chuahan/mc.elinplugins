using System.Collections.Generic;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiMotherGaia : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJenei) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.JeneiId.lang()));
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

    // Apply Spell Enhance to this ability.
    public override int GetPower(Card c)
    {
        int power = base.GetPower(c);
        return power * Mathf.Max(100 + c.Evalue(411) - c.Evalue(93), 1) / 100;
    }

    public override bool Perform()
    {
		Msg.Say("DLL path: " + typeof(ActJeneiMotherGaia).Assembly.Location);
        Msg.Say("START GAIA");
        int power = GetPower(CC);

        TC.pos.Animate(AnimeID.Quake, true);
        CC.PlaySound("spell_earthquake");
        Shaker.ShakeCam("ball");
        ActEffect.DamageEle(CC, EffectId.Earthquake, power, Element.Create(Constants.EleImpact, power / 10), new List<Point> {TC.pos}, new ActRef
        {
            act = this,
            aliasEle = Constants.ElementAliasLookup[Constants.EleImpact],
            origin = CC,
        });
        
        TC.pos.ForeachNeighbor(delegate(Point point)
        {
            ActEffect.DamageEle(CC, EffectId.Earthquake, power, Element.Create(Constants.EleImpact, power / 10), new List<Point> {point}, new ActRef
            {
                act = this,
                aliasEle = Constants.ElementAliasLookup[Constants.EleImpact],
                origin = CC,
            });
        });

        Msg.Say("END GAIA");
        return true;
    }
}