namespace PromotionMod.Stats.Ranger;

public class StanceRangerCanto : BaseStance
{
    public override bool TimeBased => true;
    public override void Tick()
    {
        // If the owner is no longer riding, cancel Canto
        if (owner.ride == null)
        {
            //TODO TEXT: Canto stop.
            Kill();
        }
    }
}