using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Berserker;

public class ActLifebreak : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatBerserker) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.BerserkerId.lang()));
            return false;
        }
        if (Act.TC == null)
        {
            return false;
        }
        return ACT.Melee.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 5,
            type = CostType.SP
        };
    }

    public override bool Perform()
    {
        new ActMeleeLifebreak().Perform(Act.CC, Act.TC);
        CC.AddCooldown(Constants.ActLifebreakId, 10);
        return true;
    }
}