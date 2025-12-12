using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The Dread Knight is an offensive Warmage promotion that uses their own life force as a resource.
///     They dominate the battlefield with relentless offensive capabilities whilst they walk the thin line between life and death.
///
///     Skill - Mana Shield (Same as Battlemage)
///     Skill - Mana Starter - Sacrifies % HP to replenish your own mana. Cooldown.
///     Skill - Life Ignition - Gains a condition that increases melee and magical damage output by 50% at the cost of 10% of their life per tick. Reduces healing recieved in this state. Lasts 5 turns.
///     Skill - Dark Aura - Sacrifies % HP to deal damage to nearby enemies as Dark damage. Cannot bring you below 1 HP. Damage dealt is Condition based HP, so it cannot be reduced via Mana Shield. No cooldown, but every time you use this, you will gain a stack of Dark Traces, which will increase the % HP cost and damage.
/// 
///     Passive - Lifetaker - Dread Knights will recover their health by 50% if they kill any enemy.
///     Passive - Conspectus of Nether - Convert Elemental Attack Spellbooks to Nether.     
/// </summary>
public class FeatDreadKnight : PromotionFeat
{
    public override string PromotionClassId => Constants.DreadKnightId;
    public override int PromotionClassFeatId => Constants.FeatDreadKnight;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.StManaShieldId,
        Constants.ActManaStarterId,
        Constants.ActLifeIgnitionId,
        Constants,ActDarkAuraId,
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.StManaShieldId, 100, false);
        c.ability.Add(Constants.ActManaStarterId, 35, false);
        c.ability.Add(Constants.ActLifeIgnitionId, 75, false);
        c.ability.Add(Constants.ActDarkAuraId, 50, false);
        
        c.ability.Add(50611, 50, false); // Add Nether Sword / Arrow
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "warmage";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
}