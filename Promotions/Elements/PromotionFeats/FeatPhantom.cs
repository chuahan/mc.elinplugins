using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * A graceful symphony of sword and spell. The Phantom is an unrelenting tide of sword and spell.
 * Phantoms focus on combinations of melee and magical attacks.
 * They specialize in successive attacks, with extravagant finisher attacks.
 */
public class FeatPhantom : PromotionFeat
{
    public override string PromotionClassId => Constants.PhantomId;
    public override int PromotionClassFeatId => Constants.FeatPhantom;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActPhantomFinisherId,
        Constants.ActSchmetterlingId, // Schmetterling - Rush Attack + Bladestorm. Rush + 5 hits doing 20% per blow. 20 Stam,  1400
        Constants.ActFolterzeitId, // Torture Time - Single Target, 5 Hits each doing 20% damage per blow. 28 Stam, 2100
        Constants.ActWolkenkratzerId, // Skyscraper - AOE Slam. 25 Stam, 1000
        Constants.ActRosenschwertId // Rose Sword - Piercing Attack. 3 Hits each doing 35% damage per blow. Piercing stabs in a 3 wide beam. 30 Stam, 1500
    };

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "swordsage";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}