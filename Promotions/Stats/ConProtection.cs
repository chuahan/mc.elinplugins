using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConProtection : BaseBuff
{
    public override ConditionType Type => ConditionType.Buff;
    public override bool WillOverride => false;

    public static int CalcProtectionAmount(int power)
    {
        return HelperFunctions.SafeMultiplier(10, 1 + power / 10);
    }

    public override void OnStartOrStack()
    {
        value = ConProtection.CalcProtectionAmount(power);
        base.OnStartOrStack();
    }

    public override void Tick()
    {
        if (value <= 0)
        {
            Kill();
        }
    }

    public void AddProtection(int amount)
    {
        value = HelperFunctions.SafeAdd(value, amount);
        OnValueChanged();
    }

    // When a new instance of Protection arrives
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