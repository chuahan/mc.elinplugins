namespace PromotionMod.Stats.Runeknight;

public class ConRunicGuard : BaseBuff
{
    public override bool TimeBased => true;

    // Does nothing but activate for Elemental Attunement.
    public override void Tick()
    {
    }
}