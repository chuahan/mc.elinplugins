namespace PromotionMod.Stats.Druid;

public class ProtectionAura : ConAura
{
    public override bool CanManualRemove => false;

    public override void ApplyInternal(Chara target)
    {
        if (target.HasCondition<ConProtection>())
        {
            target.GetCondition<ConProtection>().AddProtection(ConProtection.CalcProtectionAmount(power));
        }
        else
        {
            target.AddCondition<ConProtection>(power);
        }
    }
}