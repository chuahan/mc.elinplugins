using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// Grace in motion. The Dancer specializes in inspiring allies and bewitching foes.
/// Dancers focus on supporting allies with their beautiful dancing, while mixing in attacks during the dance to assail enemies.
/// They specialize in supporting all or a single melee oriented ally while doing light damage and statuses to enemies. 
/// </summary>
public class FeatDancer : PromotionFeat
{
    public override string PromotionClassId => Constants.DancerId;
    public override int PromotionClassFeatId => Constants.FeatDancer;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActDancePartnerId,
        Constants.ActDaggerIllusionId,
        Constants.ActSwordFouetteId,
        Constants.ActWildPirouetteId,
        Constants.StanceEnergyDanceId,
        Constants.StanceFreedomDanceId,
        Constants.StanceHealingDanceId,
        Constants.StanceMistDanceId,
        Constants.StanceSwiftDanceId
    };

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "pianist";
    }

    protected override void ApplyInternal()
    {
        // Daggers - 107
        owner.Chara.elements.ModPotential(107, 30);
        // Throwing - 108
        owner.Chara.elements.ModPotential(108, 30);
        // Dual Wield - 131
        owner.Chara.elements.ModPotential(131, 30);
    }
}