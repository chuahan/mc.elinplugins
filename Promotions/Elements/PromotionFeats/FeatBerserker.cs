using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// A glorious death awaits! Berserkers are the first to charge, the first to fight.
/// Berserkers focus on unrelenting offense, felling enemy after enemy.
/// They specialize in gaining more damage and damage reduction the more HP they are missing.
/// Skill - Bloodlust - Activates Bloodlust.
/// Skill - Sunder - Swaps debuffs with the enemy. Heals 25% HP and does 25% damage.
/// TODO: Skill - Revenge - Deals damage based on how much HP the user is missing.
/// 
/// Passive - Bloodsoaked - HP/Stamina on kill.
/// Class Condition - Gain boosts based on % HP missing and Debuff Count.
/// </summary>
public class FeatBerserker : PromotionFeat
{
    public override string PromotionClassId => Constants.BerserkerId;
    public override int PromotionClassFeatId => Constants.FeatBerserker;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActBloodlustId,
        Constants.ActSunderId
    };
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