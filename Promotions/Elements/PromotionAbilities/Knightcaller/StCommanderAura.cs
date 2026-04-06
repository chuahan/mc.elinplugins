using PromotionMod.Stats.Knightcaller;
namespace PromotionMod.Elements.PromotionAbilities.Knightcaller;

public class StCommanderAura : Ability
{
    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 1,
            type = CostType.None
        };
    }

    public override bool Perform()
    {
        if (CC.HasCondition<CommandersAura>())
        {
            CC.RemoveCondition<CommandersAura>();
        }
        else
        {
            CC.AddCondition<CommandersAura>();
        }
        return true;
    }
}