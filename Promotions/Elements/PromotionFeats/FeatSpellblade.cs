using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * The blade with magical flair. The Spellblade strikes swiftly with magical infused weapons
 * Spellblades focus on exploiting enemy weaknesses and crippling the enemy with magic.
 * They specialize in intonations and inflicting various other statuses.
 */
public class FeatSpellblade : PromotionFeat
{
    public override string PromotionClassId => Constants.SpellbladeId;
    public override int PromotionClassFeatId => Constants.FeatSpellblade;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActCrushingStrikeId,
        Constants.ActMyriadFlecheId
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "warmage";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}