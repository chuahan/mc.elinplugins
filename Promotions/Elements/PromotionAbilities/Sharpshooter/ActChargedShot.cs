using PromotionMod.Common;
using PromotionMod.Stats.Sharpshooter;
namespace PromotionMod.Elements.PromotionAbilities.Sharpshooter;

public class ActChargedShot : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSharpshooter) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SharpshooterId.lang()));
            return false;
        }

        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            type = CostType.MP,
            cost = (int)(c.mana.max * 0.25F)
        };
    }
    public override bool Perform()
    {
        int chargeAmount = (int)(CC.mana.max * 0.25F);
        CC.AddCondition<ConChargedChamber>(chargeAmount);
        return true;
    }
}