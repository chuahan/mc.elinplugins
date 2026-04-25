namespace PromotionMod.Stats;

/// <summary>
///     THIS IS SO I CAN USE MY ICONS.
/// </summary>
public class PromotionStance : BaseBuff
{
    public override string TextDuration => "";

    public override bool CanManualRemove => true;

    public override int GetPhase()
    {
        return 0;
    }
    public override void Tick()
    {
    }
}