using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PromotionMod.Common;

namespace PromotionMod.Stats.Battlemage;

/// <summary>
///     Skill - Mana Shield Stance
///         At the cost of reserving your mana (-25% max mana), you will gain a regenerating shield that will take damage before your HP does.
///         When you haven't taken damage for a time, it will regenerate at 5% a turn.
///         Shield Capacity is based off of mana reserved.
/// </summary>
public class StanceManaShield : BaseStance
{
    [JsonProperty(PropertyName = "S")] public int Stacks = 0;
    [JsonProperty(PropertyName = "R")] private int _rechargeDelay = 0;
    
    // Need to avoid taking damage for 3 turns
    private const int RechargeDelayMax = 3;

    public override bool TimeBased => true;

    public override void OnStart()
    {
        this.Stacks = this.power;
    }

    public void ModShield(int amount)
    {
        // Taking any hit, even a 0 damage hit, will incur shield delay.
        if (amount <= 0)
        {
            this._rechargeDelay = RechargeDelayMax;
        }
        this.Stacks = Math.Clamp(this.Stacks + amount, 0, this.power);
    }
    
    public override void Tick()
    {
        if (_rechargeDelay == 0)
        {
            // Recharge Shields by 5%.
            ModShield((int)(power * 0.05F));
        } else if (_rechargeDelay > 0)
        {
            _rechargeDelay--;
        }
    }
}