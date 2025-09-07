using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiMove : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJenei) == 0) return false;
        if (TC.isChara && TC.isRestrained) return false; // Cannot move restrained characters.
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            type = CostType.MP,
            cost = 2
        };
    }

    public override bool Perform()
    {
        if (TryMoveFrom(TC, CC.pos) == Card.MoveResult.Success)
        {
            // TODO (P3) Render hand moving it.
            return true;
        }
        return false;
    }

    public Card.MoveResult TryMoveFrom(Card target, Point p)
    {
        Point newPoint = p.Copy();
        int num1 = p.x - target.pos.x;
        int num2 = p.z - target.pos.z;
        if (num1 > 1)
            num1 = 1;
        else if (num1 < -1)
            num1 = -1;
        if (num2 > 1)
            num2 = 1;
        else if (num2 < -1)
            num2 = -1;
        if (num1 == 0 && num2 == 0)
        {
            num1 = EClass.rnd(3) - 1;
            num2 = EClass.rnd(3) - 1;
        }
        newPoint.Set(target.pos);
        newPoint.x -= num1;
        newPoint.z -= num2;
        return newPoint is { IsValid: true, HasChara: false, IsBlocked: false, IsInBounds: false } ? target._Move(newPoint, Card.MoveType.Force) : Card.MoveResult.Fail;
    }
}