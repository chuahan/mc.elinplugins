namespace PromotionMod.Stats.Dancer;

public class ConInfatuation : Timebuff
{
    public override void Tick()
    {
        if (!Owner.IsPC)
        {
            Owner.ModAffinity(pc, 1, false, true);
        }
        base.Tick();
    }
}