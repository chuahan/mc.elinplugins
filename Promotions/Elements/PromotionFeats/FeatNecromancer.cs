using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The speaker of the dead. Necromancers are wizards who have forsaken the elements, and wield death itself.
///     Necromancers focus on the summoning and utilization of a mass army of the dead.
///     They specialize in calling forth armies of minions, using them to turn the tides of battle through undeath or
///     death.
///     Skill - Beckon of the Dead - Marks an enemy for re-animation. On death, they will produce a skeleton based on their
///     level.
///     Skill - Blessing of the dead - Kills an undead minion. Whatever their HP was remaining is used to heal the
///     Necromancer's nearby allies.
///     If the healed target was at full HP, the healing is applied as Protection.
///     Skill - Corpse Explosion - Kills an undead minion. Whatever their HP was remaining is used to damage nearby enemies
///     with Nether damage.
///     Passive - Book Conversion - Converts Summon Spellbooks into summon Skeleton Soldier
///     Archers / Warriors / Mages
///     Passive - Bone Armor - For every active undead summon, Necromancer gains damage reduction.
/// </summary>
public class FeatNecromancer : PromotionFeat
{
    public override string PromotionClassId => Constants.NecromancerId;
    public override int PromotionClassFeatId => Constants.FeatNecromancer;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActBeckonOfTheDeadId,
        Constants.ActBlessingOfTheDeadId,
        Constants.ActCorpseExplosionId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActBeckonOfTheDeadId, 75, false);
        c.ability.Add(Constants.ActBlessingOfTheDeadId, 50, false);
        c.ability.Add(Constants.ActCorpseExplosionId, 50, false);
        c.ability.Add(Constants.SpSummonSkeleton, 90, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "wizard";
    }

    protected override void ApplyInternal()
    {
        // Anatomy - 286
        //owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        //owner.Chara.elements.ModPotential(304, 30);
    }
}