namespace PromotionMod.Elements.Alraune;

public class ConDigestingPrey : BaseDebuff
{
    public override ConditionType Type => ConditionType.Sentence;
    public override bool TimeBased => true;
    public override int GetPhase()
    {
        return 0;
    }
    public override bool CanStack(Condition c)
    {
        return false;
    }

    public override void Tick()
    {
        if (owner.hunger.value > 0)
        {
            // Hunger instead works to reduce this condition.
            Mod(0 - owner.hunger.value);
            owner.hunger.value = 0; // Set it back to 0.
        }
        base.Tick();
    }
}