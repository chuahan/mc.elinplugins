using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The Dread Knight is an offensive Warmage promotion that uses their own life force as a resource.
///     They dominate the battlefield with relentless offensive capabilities whilst they walk the thin line between life and death.
///
///     TODO Do we want them to have Mana Shield?
/// 
///     Skill - Mana Starter - Sacrifices % HP to replenish your own mana. Cooldown.
///         Mana Starter gains more % HP cost based on Dark Traces.
///     Skill - Life Ignition Stance - Stance that increases melee and magical damage output by 50% at the cost of
///         10% of their life per tick. Reduces healing received in this state.
///         Will end if user's HP goes below 10%.
///     Skill - Dark Aura - Sacrifices % HP to deal damage to nearby enemies as Dark damage.
///         Dark Aura gains radius based on Dark Traces
///
///     Skill - Dark Armor - Consumes Dark Traces to add Protection to self. If above 5 stacks, allies within Radius gain half Protection. 
/// 
///     HP Sacrificing abilities (Mana Starter and Dark Aura) cannot be used if it would kill you.
///     Damage dealt is Condition based HP, so it cannot be reduced via Mana Shield.
///     No cooldown, but every time you use them, you will gain a stack of Dark Traces,
///     which increases the HP Cost as well as well as the effect.
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
        Constants.StLifeIgnitionId,
        Constants.ActDarkAuraId,
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.StManaShieldId, 100, false);
        c.ability.Add(Constants.ActManaStarterId, 35, false);
        c.ability.Add(Constants.StLifeIgnitionId, 75, false);
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