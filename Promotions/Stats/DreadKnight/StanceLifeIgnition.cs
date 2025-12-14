using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities.DreadKnight;

namespace PromotionMod.Stats.DreadKnight;

public class StanceLifeIgnition : BaseStance
{
    public override void Tick()
    {
        // If the users HP falls below 10%, automatically exit Life Ignition Stance and add a cooldown.
        if (Owner.hp <= (int)(Owner.MaxHP * 0.25F))
        {
            // TODO Text - Say Life Ignition stops
            Owner.AddCooldown(Constants.StLifeIgnitionId, 5);
            Kill();   
        }
        
        // Reduce owner's HP by 10%.
        int cost = (int)(Owner.MaxHP * 0.1F);
        Owner.DamageHP(cost, AttackSource.Condition);
    }
}