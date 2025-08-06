using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * Light our way! The Seer brings down the light of judgement upon their enemy.
 * Seer focus on healing allies while casting holy magic at the enemies.
 * They specialize in casting advanced curative magic and applying disabling debuffs, as well as empowered Holy Magic.
 */
public class FeatSeer : PromotionFeat
{
    public override string PromotionClassId => Constants.SeerId;
    public override int PromotionClassFeatId => Constants.FeatSeer;
    public override List<int> PromotionAbilities => new List<int>
    {
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "priest";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}