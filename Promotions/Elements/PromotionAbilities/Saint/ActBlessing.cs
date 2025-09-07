using PromotionMod.Common;
using PromotionMod.Stats.Saint;
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
        // TODO (P3) Change to cost % of mana.
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        // Apply Blessing based off of Faith.
        TC.Chara.AddCondition<ConBlessing>(CC.Evalue(Constants.FaithId));
        return true;
    }
}