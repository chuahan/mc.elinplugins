namespace PromotionMod.Stats;

public class ConSerpentineAgility : BaseBuff
{
    public override ConditionType Type => ConditionType.Buff;
    
    // Overriding TimeBased to be true allows this condition to tick on the global turn timer instead of individual ticks
    public override bool TimeBased => true;
    
    // Since this is more of a "Toggle" ability, this hides the number that normally would show the duration.
    public override string TextDuration => "";
    
    public override void Tick()
    {
        // I overrided the base Tick() function with an empty function. My reasoning is that this ability will remain active until the Naga actually performs a different action.
    }
}