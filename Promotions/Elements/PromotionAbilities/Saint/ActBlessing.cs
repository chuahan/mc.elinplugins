using PromotionMod.Common;

namespace PromotionMod.Elements.PromotionAbilities.Saint;

public class ActBlessing : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSaint) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SaintId.lang()));
            return false;
        }

        return base.CanPerform();
    }
    
    public override Cost GetCost(Chara c)
    {
        // TODO: Change to cost % of mana.
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        TC.Chara.AddCondition<ConBlessing>(CC.Evalue(306 /* Faith */));    
        return true;
    }
}