using PromotionMod.Common;
namespace PromotionMod.Stats.Harbinger;

public class ConDisorientingMiasma : ConHarbingerMiasma
{
    public override int Element => Constants.EleSound;
    public override void ApplyCondition(Chara c)
    {
        c.AddCondition<ConConfuse>(power);
    }
}