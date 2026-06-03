using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConDebilitatingMiasma : ConHarbingerMiasma
{
    public override int Element => Constants.EleImpact;
    public override void ApplyCondition(Chara c)
    {
        c.AddCondition<ConWeakness>(power);
    }
}