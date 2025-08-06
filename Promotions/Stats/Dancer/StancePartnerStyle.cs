namespace PromotionMod.Stats.Dancer;

public class StancePartnerStyle : BaseStance
{
    public Chara DancePartner;

    public override void Tick()
    {
        if (DancePartner == null)
        {
            Kill();
        }
    }
}