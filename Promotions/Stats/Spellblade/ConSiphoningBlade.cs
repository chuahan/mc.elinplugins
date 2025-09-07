namespace PromotionMod.Stats.Spellblade;

public class ConSiphoningBlade : BaseBuff
{
    public override void Tick()
    {
        // This condition really shouldn't stick around past it's attack.
        Kill();
    }
}