namespace PromotionMod.Stats;

public class ConAfterimage : BaseBuff
{
    public override ConditionType Type => ConditionType.Buff;

    public override void Tick()
    {
        if (value <= 0)
        {
            Kill();
        }
    }

    // When a new instance of Afterimage arrives
    // It will evaluate itself against the new condition in OnStacked.
    public override bool CanStack(Condition c)
    {
        return c.GetType() == GetType();
    }

    public override void OnStacked(int p)
    {
        if (p > value)
        {
            value = p;
        }
        SetPhase();
    }
}