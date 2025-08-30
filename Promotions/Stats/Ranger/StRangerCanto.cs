namespace PromotionMod.Stats.Ranger;

public class StRangerCanto : BaseStance
{
    public override void Tick()
    {
        // If the users HP falls below 25%, automatically exit Vanguard Stance.
        if (Owner.hp <= (int)(Owner.MaxHP * 0.25F)) this.Kill();
    }
}