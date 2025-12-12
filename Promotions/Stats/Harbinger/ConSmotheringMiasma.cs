using PromotionMod.Common;
namespace PromotionMod.Stats.Harbinger;

public class ConSmotheringMiasma : ConHarbingerMiasma
{
    public override int Element => Constants.EleMind;
    public override void ApplyCondition(Chara c)
    {
        c.AddCondition<ConParalyze>(power);
    }
}