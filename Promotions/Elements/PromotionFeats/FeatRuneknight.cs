using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The sword of mystical judgement. The Runeknights etch magic itself into their skin and armor, allowing them to
///     harness surrounding magical energy.
///     Runeknights focus on both physical and magical attacks, able to turn the foes spells against them.
///     They specialize in being exceptionally resistant against enemy magic spells, absorbing them to empower themselves.
/// </summary>
public class FeatRuneknight : PromotionFeat
{
    public override string PromotionClassId => Constants.RuneknightId;
    public override int PromotionClassFeatId => Constants.FeatRuneknight;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActRunicGuardId
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