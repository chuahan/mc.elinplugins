using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConChillingMiasma : ConHarbingerMiasma
{
    public override int Element => Constants.EleCold;
    public override void ApplyCondition(Chara c)
    {
        c.AddCondition<ConFreeze>(power);
    }
}