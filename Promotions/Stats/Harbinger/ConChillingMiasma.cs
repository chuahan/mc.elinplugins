using PromotionMod.Common;
namespace PromotionMod.Stats.Harbinger;

public class ConChillingMiasma : ConHarbingerMiasma
{
    public override int Element => Constants.EleCold;
    public override void ApplyCondition(Chara c)
    {
        c.AddCondition<ConFreeze>(power);
    }
}