using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiMove : AI_TargetCard
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJenei) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.JeneiId.lang()));
            return false;
        }
        if (Act.TC == null) return false;
        return this.IsValidTC(Act.TC);
    }
    
    public override bool IsValidTC(Card c)
    {
        if (EClass._zone.IsUserZone)
        {
            return false;
        }
        if (c.isThing & (EClass._zone is Zone_LittleGarden))
        {
            return false;
        }
        if (c.isChara && c.Chara.isRestrained) return false;
        return true;
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
            // FX: Render hand moving it.
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
        if (newPoint is { IsValid: true, HasChara: false, IsBlocked: false, IsInBounds: true })
        {
            return target._Move(newPoint, Card.MoveType.Force);
        } 
        return Card.MoveResult.Fail;
    }
}