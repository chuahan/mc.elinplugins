namespace PromotionMod.Stats.Trickster;

public class ConDespair : BadCondition
{
    public override bool TimeBased => true;

    public override void Tick()
    {
        owner.DamageHP((long)(owner.MaxHP * 0.1F), AttackSource.Condition);
        base.Tick();
    }
}