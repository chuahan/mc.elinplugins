using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The Dread Knight is an offensive Warmage promotion that uses their own life force as a resource.
///     They dominate the battlefield with relentless offensive capabilities whilst they walk the thin line between life and death.
/// 
///     Skill - Mana Starter - Sacrifices % HP to replenish your own mana. Cooldown.
///         Mana Starter gains more % HP cost based on Dark Traces.
///     Skill - Life Ignition Stance - Stance that increases melee and magical damage output by 50% at the cost of
///         10% of their life per tick. Reduces healing received in this state.
///         Will end if user's HP goes below 10%.
///     Skill - Dark Burst - Sacrifices % HP to deal damage to nearby enemies as Dark damage.
///         Dark Burst gains radius based on Dark Traces
///
///     Condition - Dark Traces
///         Both Mana Starter and Dark Burst add Dark Traces.
///         Having Dark Traces will increase your outgoing spell and melee damage based on your HP missing %.
///             Every 10% missing HP will increase your outgoing damage by 1% per stack of dark traces.
///         
///     Skill - Dark Barrier - 
///         Converts your Dark Traces into Protection.
///         Amount scales based on a % of your HP, each stack of Dark Trace counts as 5%.
/// 
///     HP Sacrificing abilities (Mana Starter and Dark Burst) cannot be used if it would kill you.
///     Damage dealt is condition-based, so it cannot be reduced via Mana Shield.
///     No cooldown, but every time you use them, you will gain a stack of Dark Traces,
///     which increases the HP Cost as well as the effect.
/// 
///     Passive - Lifetaker - Dread Knights will recover their health by 25% if they kill any enemy.
///     Passive - Conspectus of Nether - Convert Elemental Attack Spellbooks to Nether.
/// </summary>
public class FeatDreadKnight : PromotionFeat
{
    public override string PromotionClassId => Constants.DreadKnightId;
    public override int PromotionClassFeatId => Constants.FeatDreadKnight;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActManaStarterId,
        Constants.StLifeIgnitionId,
        Constants.ActDarkBurstId,
        Constants.ActDarkBarrierId,
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActManaStarterId, 35, false);
        c.ability.Add(Constants.StLifeIgnitionId, 75, false);
        c.ability.Add(Constants.ActDarkBurstId, 50, false);
        c.ability.Add(Constants.ActDarkBarrierId, 50, false);
        
        c.ability.Add(51006, 50, false); // Nether Sword
        c.ability.Add(50506, 50, false); // Nether Arrow
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