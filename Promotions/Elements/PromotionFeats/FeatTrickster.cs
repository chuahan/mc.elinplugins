using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The nightmare harlequin. A master in the illusion arts, Tricksters specialize in deception and debilitation.
///     Tricksters focus on sowing discord and chaos through the enemy ranks through traps and illusions.
///     They specialize in applying and then exploiting debuffs to deal damage or buff their own forces.
///     Skill - Arcane Trap - Sets an explosive Arcane trap. These will inflict various debuffs on the enemy.
///     Arcane traps on trigger will affect all hostiles within a 2 radius.
///     Skill - Detonate Traps - Causes the targeted Faction Trap to detonate on command. 0 Cost.
///     Skill - Diversion - Summons 3 Phantom Tricksters.
///     Skill - Reversal - Targets an enemy and consumes all the debuffs on them. Protection is added to the Trickster's
///     forces based on the # removed.
///     Passive - Phantom Trickster - When the Trickster dodges attacks, an illusion minion is created with taunt.
///     The Phantom Trickster has Taunt, and is immune to all magical attacks, but only has 1 HP.
///     On Death, if their killer is hostile, they will inflict one of the Trickster Debuffs.
///     Trickster Debuffs available:
///     ConDim
///     ConInsane
///     ConConfusion
///     ConFear
///     ConWeakness
///     ConWeakResEle
///     ConNightmare
///     ConParanoia - Hexer Debuff. Causes them to prioritize attacking allies.
///     ConDespair - Prevents Healing.
///     ConVulnerability -
/// </summary>
public class FeatTrickster : PromotionFeat
{
    public override string PromotionClassId => Constants.TricksterId;
    public override int PromotionClassFeatId => Constants.FeatTrickster;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActArcaneTrapId,
        Constants.ActDetonateTrapId,
        Constants.ActDiversionId,
        Constants.ActReversalId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActArcaneTrapId, 80, false);
        c.ability.Add(Constants.ActDiversionId, 75, false);
        c.ability.Add(Constants.ActReversalId, 50, false);
    }


    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "thief";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}