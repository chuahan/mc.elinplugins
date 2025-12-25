namespace PromotionMod.Stats.Druid;

public class HealingAura : ConAura
{
    public override bool CanManualRemove => false;

    public override void ApplyFriendly(Chara target)
    {
        target.AddCondition<ConGreaterRegen>(power);
    }
}