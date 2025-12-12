using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiPly : Ability
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
        float healMultiplier = HelperFunctions.SigmoidScaling(this.GetPower(CC), 0.08F, 0.4F, 4600);
        int healAmount = (int)(TC.MaxHP * healMultiplier);
        TC.HealHP(healAmount, HealSource.Magic);
        return true;
    }
}