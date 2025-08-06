namespace PromotionMod.Stats.Hermit;

/// <summary>
///     Assassinate Condition
///     It's a hacky way to do it. But when casting Assassinate, this will be added onto the Hermit.
///     The patch then uses this to guarantee accuracy
///     When the Attack finishes, the condition is removed.
/// </summary>
public class ConDeathbringer : BaseBuff
{
    public override void Tick()
    {
    }
}