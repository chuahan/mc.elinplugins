using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Saint;

public class ActHandOfGod : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatSaint;
    public override string PromotionString => Constants.SaintId;
    public override int Cooldown => 30;
    public override int AbilityId => Constants.ActHandOfGodId;

    public override bool Perform()
    {
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, false))
        {
            int power = (int)HelperFunctions.SigmoidScaling(CC.Evalue(SKILL.faith), .25F, 5F);
            power += GetPower(CC);
            TC.HealHP(power, HealSource.Magic);
            TC.Chara.AddCondition<ConGreaterRegen>(CC.Evalue(SKILL.faith));
        }

        CC.AddCooldown(AbilityId, Cooldown);
        return true;
    }
}