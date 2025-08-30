namespace PromotionMod.Stats.Runeknight;

public class ConWardingRune : BaseBuff
{
    public override bool TimeBased => true;
    
    // Stack based Condition instead.
    public override void Tick()
    {
    }
    
    public override bool CanStack(Condition c)
    {
        return true;
    }
}