using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// Practitioners of the forbidden arts. Hexers have learned creative new ways to cause pain and suffering.
/// Hexers focus on weakening the enemies with curses and damage over time effects.
/// They specialize in applying debuffs, then exploiting those debuffs to deal damage.
/// </summary>
public class FeatHexer : PromotionFeat
{
    public override string PromotionClassId => Constants.HexerId;
    public override int PromotionClassFeatId => Constants.FeatHexer;
    public override List<int> PromotionAbilities => new List<int>
    {
        
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "witch";
    }

    protected override void ApplyInternal()
    {
        // Alchemy - 257
        owner.Chara.elements.ModPotential(257, 30);
        // Casting
    }
}