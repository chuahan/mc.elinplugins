using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// The speaker for nature. Druids are able to call forth vitality even in the most barren places.
/// Druids focus on magics related to nature, summoning plantlife to aid their cause.
/// They specialize in supporting their teams through casting various control spells.
/// Skill - Sow Wrath Seeds - Summon Offensive flowers
/// Skill - Sow Warm Seeds - Summon Defensive or Support flowers
/// TODO (P3) Give them another skill Maybe?
/// 
/// Passive - Conspectus of Nature - Convert Summon books to Summon Tree Ent Warrior
/// </summary>
public class FeatDruid : PromotionFeat
{
    public override string PromotionClassId => Constants.DruidId;
    public override int PromotionClassFeatId => Constants.FeatDruid;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActSowWarmSeedsId,
        Constants.ActSowWrathSeedsId
    };
    
    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActSowWarmSeedsId, 60, false);
        c.ability.Add(Constants.ActSowWrathSeedsId, 60, false);
        c.ability.Add(Constants.SpSummonTreeEntId, 80, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "farmer";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}