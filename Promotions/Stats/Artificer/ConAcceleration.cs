namespace PromotionMod.Stats.Artificer;

public class ConAcceleration : BaseBuff
{
    public override void Tick()
    {
        // Having this condition ticks cooldowns faster.
        owner.TickCooldown();
        base.Tick();
    }
}