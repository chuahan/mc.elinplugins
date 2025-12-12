using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The head is where the man is. The Headhunter is infallible, the axe will drop sooner or later.
///     Headhunters focus on opportunistic killing blows to strengthen themselves.
///     They specialize in last hitting enemies to grant themselves stacking buffs, slowly becoming an unstoppable force as
///     the battle goes on.
///
///     // TODO: Rename to Reaper. Cursed Equipment specialist
///     // Gains double the effect from cursed equipment? How hard is this going to be?
/// 
///     Skill - Execute. - Melee attack that has a 100% chance to kill a target at or below 25% HP.
///     If the cull was successful, boosts armor and speed of allies.
///     Skill - Reap - Melee attack that gains increased effects against bad debuffed enemies. Does increased damage
///     against full HP enemies. Inflicts armor down.
///
///     Passive - Trophy Hunter - Gain increased damage against rare enemies.
///     Passive - Headhunting - Whenever you land the killing blow against an enemy, you gain a stack of headhunter.
///     When you slay an enemy, if they have any buffs, you gain a copy of the buff.
///     Headhunter stacks boost all stats.
///     Reduces cooldowns of Reap and Execute.
/// </summary>
public class FeatHeadhunter : PromotionFeat
{
    public override string PromotionClassId => Constants.HeadhunterId;
    public override int PromotionClassFeatId => Constants.FeatHeadhunter;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActExecuteId,
        Constants.ActReapId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActExecuteId, 75, false);
        c.ability.Add(Constants.ActReapId, 75, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "executioner";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
}