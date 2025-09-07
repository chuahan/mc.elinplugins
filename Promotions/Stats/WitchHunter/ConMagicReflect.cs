namespace PromotionMod.Stats.WitchHunter;

public class ConMagicReflect : Condition
{
    public override bool CanManualRemove => true;

    public override void Tick()
    {
        // No ticking. Only gets reduced when it reflects a spell.
    }
}