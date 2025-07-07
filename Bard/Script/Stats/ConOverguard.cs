using BardMod.Common.HelperFunctions;

namespace BardMod.Stats;

public class ConOverguard : BaseBuff
{
    public override bool WillOverride => true;

    public int CalcOverguardAmount()
    {
        return HelperFunctions.SafeMultiplier(10, 1 + power / 10);
    }
    
    public override void OnStartOrStack()
    {
        value = CalcOverguardAmount();
        base.OnStartOrStack();
    }

    public override void Tick()
    {
        if (this.value <= 0)
        {
            Kill();
        }
    }

    public void AddOverGuard(int amount)
    {
        this.value = HelperFunctions.SafeAdd(this.value, amount);
        this.OnValueChanged();
    }
    
    public override bool CanStack(Condition c)
    {
        if (c.GetType() == this.GetType())
        {
            return c.value > this.value;
        }
        return false;
    }
}