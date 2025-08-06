using PromotionMod.Common;
namespace PromotionMod.Stats.Harbinger;

public class ConRendingMiasma : ConHarbingerMiasma
{
    public override int Element => Constants.EleCut;
    public override void ApplyCondition(Chara c)
    {
        c.AddCondition<ConBleed>(power);
    }
}