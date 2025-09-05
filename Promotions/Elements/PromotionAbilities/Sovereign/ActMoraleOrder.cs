using PromotionMod.Common;
using PromotionMod.Stats.Sovereign;

namespace PromotionMod.Elements.PromotionAbilities.Sovereign;

public class ActMoraleOrder : ActSovereignOrder
{
    protected override string OrderType => "morale";
    protected override int CooldownId => Constants.ActMoraleOrderId;
    public override void AddLawCondition(Chara chara, int stacks)
    {
        chara.AddCondition<ConOrderVictory>(stacks);
    }
    public override void AddChaosCondition(Chara chara, int stacks)
    {
        chara.AddCondition<ConOrderDeath>(stacks);
    }
}