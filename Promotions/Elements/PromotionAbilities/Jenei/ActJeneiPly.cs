using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiPly : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatJenei;
    public override string PromotionString => Constants.JeneiId;
    public override int AbilityId => Constants.ActJeneiPlyId;

    public override bool Perform()
    {
        float healMultiplier = HelperFunctions.SigmoidScaling(GetPower(CC), 0.08F, 0.4F, 4600);
        int healAmount = (int)(TC.MaxHP * healMultiplier);
        TC.HealHP(healAmount, HealSource.Magic);
        return true;
    }
}