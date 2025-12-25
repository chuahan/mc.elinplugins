namespace PromotionMod.Stats.Druid;

public class ProtectionAura : ConAura
{
    public override bool CanManualRemove => false;

    public override void ApplyFriendly(Chara target)
    {
        target.AddCondition<ConProtection>(power);
    }
}