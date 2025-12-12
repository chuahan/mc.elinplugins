namespace PromotionMod.Stats.Artificer;

public class ConTitanProtocol : BaseBuff
{
    public override void Tick()
    {
        if (owner.host == null)
        {
            this.Kill();
        }
    }
}