using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats.Battlemage;

/// <summary>
///     Skill - Mana Shield Stance
///     At the cost of reserving your mana (-25% max mana), you will gain a regenerating shield that will take damage
///     before your HP does.
///     When you haven't taken damage for a time, it will regenerate at 5% a turn.
///     Shield Capacity is based off of mana reserved.
/// </summary>
public class StanceManaShield : BaseStance
{
    // Need to avoid taking damage for 3 turns
    private const int RechargeDelayMax = 3;
    [JsonProperty(PropertyName = "R")] private int _rechargeDelay;
    [JsonProperty(PropertyName = "S")] public int Stacks;

    public override bool TimeBased => true;
    
    public override string TextDuration => "" + Stacks;

    public override void OnStart()
    {
        Stacks = power;
    }

    public void ModShield(int amount)
    {
        // Taking any hit, even a 0 damage hit, will incur shield delay.
        if (amount <= 0)
        {
            _rechargeDelay = RechargeDelayMax;
        }
        Stacks = Math.Clamp(Stacks + amount, 0, power);
    }

    public override void Tick()
    {
        if (_rechargeDelay == 0)
        {
            // Recharge Shields by 5%.
            ModShield((int)(power * 0.05F));
        }
        else if (_rechargeDelay > 0)
        {
            _rechargeDelay--;
        }
    }
    
    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintManaShield".lang(this.Stacks.ToString(), power.ToString()));
        if (_rechargeDelay > 0)
        {
            list.Add("hintManaShieldDelay".lang(this._rechargeDelay.ToString()));
        }
    }
}