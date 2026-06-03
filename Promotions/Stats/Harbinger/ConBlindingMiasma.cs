using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConBlindingMiasma : ConHarbingerMiasma
{
    public override int Element => Constants.EleDarkness;
    public override void ApplyCondition(Chara c)
    {
        c.AddCondition<ConBlind>(power);
    }
}