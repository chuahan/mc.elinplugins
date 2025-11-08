using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     A glorious death awaits! Berserkers are the first to charge, the first to fight.
///     Berserkers focus on unrelenting offense, felling enemy after enemy.
///     They specialize in gaining more damage and damage reduction the more HP they are missing.
///     Skill - Bloodlust - Activates Bloodlust. Boosts stats, but silences yourself
///         Costs 25% of current HP to activate, but regenerates 5% HP a turn.
///         Adds Counter to the character.
///     Skill - Sunder - Swaps debuffs with the enemy. Heals 25% HP and does 25% damage.
///     Skill - Lifebreak - Deals damage based on how much HP the user is missing.
///     10 Turn Cooldown.
///     Passive - Bloodsoaked - HP/Stamina on kill.
///     Class Condition - Gain boosts based on % HP missing and Debuff Count.
/// </summary>
public class FeatBerserker : PromotionFeat
{
    public override string PromotionClassId => Constants.BerserkerId;
    public override int PromotionClassFeatId => Constants.FeatBerserker;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActBloodlustId,
        Constants.ActSunderId,
        Constants.ActLifebreakId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActBloodlustId, 70, false);
        c.ability.Add(Constants.ActSunderId, 25, false);
        c.ability.Add(Constants.ActLifebreakId, 30, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "warrior";
    }

    protected override void ApplyInternal()
    {
        // Reading - 285
        owner.Chara.elements.ModPotential(285, 30);
    }
}