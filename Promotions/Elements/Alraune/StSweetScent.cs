namespace PromotionMod.Elements.Alraune;

public class StSweetScent : Ability
{
    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 0,
            type = CostType.None
        };
    }

    public override bool Perform()
    {
        if (CC.HasCondition<AuraSweetScent>())
        {
            CC.RemoveCondition<AuraSweetScent>();
        }
        else
        {
            CC.AddCondition<AuraSweetScent>();
        }
        return true;
    }
}