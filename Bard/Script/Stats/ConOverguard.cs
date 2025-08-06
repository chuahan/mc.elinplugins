using BardMod.Common.HelperFunctions;
namespace BardMod.Stats;

public class ConOverguard : BaseBuff
{
    public override bool WillOverride => false;

    public static int CalcOverguardAmount(int power)
    {
        return HelperFunctions.SafeMultiplier(10, 1 + power / 10);
    }

    public override void OnStartOrStack()
    {
        value = ConOverguard.CalcOverguardAmount(power);
        base.OnStartOrStack();
    }

    public override void Tick()
    {
        if (value <= 0)
        {
            Kill();
        }
    }

    public void AddOverguard(int amount)
    {
        value = HelperFunctions.SafeAdd(value, amount);
        OnValueChanged();
    }

    // When a new instance of Overguard arrives
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