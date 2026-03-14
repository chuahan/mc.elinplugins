using System;
using Newtonsoft.Json;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Stats;

public class ConProtection : BaseBuff
{
    /// <summary>
    /// When adding Protection for the first time, the power is used as the maximum protection amount.
    /// When Protection is replenished, it will go up to the maximum amount.
    /// If the incoming Protection Replenishment is HIGHER than the maximum amount, maxShield is increased to that amount.
    /// </summary>
    [JsonProperty(PropertyName = "S")] private int _maxShield = 1;
    
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    
    public static int CalcAmount(int power)
    {
        return HelperFunctions.SafeMultiplier(4, 10 + (power/45) + (power/30));
    }
    
    public override ConditionType Type => ConditionType.Buff;

    public override void OnStart()
    {
        value = HelperFunctions.SafeAdd(value, ConProtection.CalcAmount(power));
        _maxShield = value;
        base.OnStart();
    }

    public override void Tick()
    {
        if (value <= 0)
        {
            Kill();
        }
    }

    public void AddProtection(int amount, bool scale=false) 
    {
        value = Math.Min(_maxShield, scale ? HelperFunctions.SafeAdd(value, ConProtection.CalcAmount(amount)) : HelperFunctions.SafeAdd(value, amount));
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
            value = Math.Min(_maxShield, HelperFunctions.SafeAdd(value, ConProtection.CalcAmount(p)));
        }

        SetPhase();
    }
}