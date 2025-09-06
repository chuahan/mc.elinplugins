namespace PromotionMod.Stats.WarCleric;

public class ConSolBlade : BaseBuff
{
    public override void Tick()
    {
        // This condition really shouldn't stick around past it's attack.
        this.Kill();
    }
}