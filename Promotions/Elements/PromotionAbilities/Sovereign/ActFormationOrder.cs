using PromotionMod.Common;
using PromotionMod.Stats.Sovereign;

namespace PromotionMod.Elements.PromotionAbilities.Sovereign;

public class ActFormationOrder : ActSovereignOrder
{
    protected override string OrderType => "formation";
    protected override int CooldownId => Constants.ActFormationOrderId;
    public override void AddLawCondition(Chara chara, int stacks)
    {
        chara.AddCondition<ConOrderSword>(stacks);
    }
    public override void AddChaosCondition(Chara chara, int stacks)
    {
        chara.AddCondition<ConOrderBarricade>(stacks);
    }
}