using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The story of ancient valor. The Knightcaller performs in otherworldly songs that calls forth aid from a ghostly
///     knight order.
///     Knightcallers focus on summoning a force of knights from the past through song to go to war once again.
///     They specialize in summoning special minions and have minion specific support spells.
///     Skill - Enrage Spirits - Enrages all your summons, boosting their offensive capabilities.
///     Skill - Rally Spirits - Recalls all your summons to your side, healing them, and boosting their defensive
///     capabilities.
///     Skill - Summon Knight - Summons a Spirit Knight to aid you in battle. If you do not have an active captain, has a
///     25% chance of summoning a Knight Captain instead.
/// </summary>
public class FeatKnightcaller : PromotionFeat
{
    public override string PromotionClassId => Constants.KnightcallerId;
    public override int PromotionClassFeatId => Constants.FeatKnightcaller;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActSpiritRageId,
        Constants.ActSpiritRallyId,
        Constants.ActSummonKnightId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActSpiritRageId, 75, false);
        c.ability.Add(Constants.ActSpiritRallyId, 75, false);
        c.ability.Add(Constants.ActSummonKnightId, 80, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "pianist";
    }

    protected override void ApplyInternal()
    {
        // Instrument - 286
        //owner.Chara.elements.ModPotential(286, 30);
    }
}