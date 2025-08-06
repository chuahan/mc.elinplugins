using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * The speaker of the dead. Necromancers are wizards who have forsaken the elements, and wield death itself.
 * Necromancers focus on the summoning and utilization of a mass army of the dead.
 * They specialize in calling forth armies of minions, using them to turn the tides of battle through undeath or death.
 */
public class FeatNecromancer : PromotionFeat
{
    public override string PromotionClassId => Constants.NecromancerId;
    public override int PromotionClassFeatId => Constants.FeatNecromancer;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActBeckonOfTheDeadId,
        Constants.ActDieForMeId,
        Constants.ActCorpseExplosionId
    };

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "wizard";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}