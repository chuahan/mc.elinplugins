using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConProtection : BaseBuff
{
    /// <summary>
    /// When adding Protection for the first time, the power is used as the maximum protection amount.
    /// When Protection is replenished, it will go up to the maximum amount.
    /// If the incoming Protection Replenishment is HIGHER than the maximum amount, maxShield is increased to that amount.
    /// </summary>
    [JsonProperty(PropertyName = "S")] private int _maxShield = 1;
    
    public override ConditionType Type => ConditionType.Buff;

    public override void OnStartOrStack()
    {
        value = power;
        _maxShield = value;
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

    public override bool CanStack(Condition c)
    {
        return c.GetType() == GetType();
    }

    public override void OnStacked(int p)
    {
        if (p > _maxShield)
        {
            _maxShield = p;
            value = p;
        }
        else
        {
            value = HelperFunctions.SafeAdd(value, p);   
        }

        SetPhase();
    }
}