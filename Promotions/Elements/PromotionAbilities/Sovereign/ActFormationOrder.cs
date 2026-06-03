using PromotionMod.Common;
using PromotionMod.Stats;

namespace PromotionMod.Elements;

public class ActFormationOrder : ActSovereignOrder
{
    protected override string OrderType => "formation";
    protected override int CooldownId => Constants.ActFormationOrderId;
    public override int AbilityId => Constants.ActFormationOrderId;

    public override void AddLawCondition(Chara chara, int stacks)
    {
        chara.AddCondition<ConOrderBarricade>(stacks);
    }
    public override void AddChaosCondition(Chara chara, int stacks)
    {
        chara.AddCondition<ConOrderSword>(stacks);
    }
}