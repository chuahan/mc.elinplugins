namespace PromotionMod.Stats.Jenei;

public class ConWatersOfLife: Timebuff
{
    // Applies healing over time doing 60%, 50%, 40%, 30%, and 20%.
    public override void Tick()
    {
        float healingAmount = this.value switch
        {
            5 => 0.6f,
            4 => 0.5f,
            3 => 0.4f,
            2 => 0.3f,
            _ => 0.2f,
        };
        
        owner.HealHP((int)(owner.MaxHP * healingAmount), HealSource.HOT);
        base.Tick();
    }
}