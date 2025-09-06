using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// A wonderous amalgamation of magic and technology. The Artificer focuses on the production of magical tools.
/// Artificers focus on the development and creation of magical tools of varied sizes and use.
/// They specialize in the creation, use, and recharging of magical devices. They also are able to produce their own
/// masterpiece: The Golem.
/// PC PROMOTION ONLY
/// They learn the recipe for the Artificer Altar, which allows them to craft items.
/// Skill - Simple Synthesis - Reloads magical devices/guns for you and your party.
/// Skill - Improvised Brew - Throw a random potion at enemy or ally.
/// Passive - Material Hunter - When Sun/Earth/Mana Crystals are mined out of walls, drops two instead.
/// </summary>
public class FeatArtificer : PromotionFeat
{
    public override string PromotionClassId => Constants.ArtificerId;
    public override int PromotionClassFeatId => Constants.FeatArtificer;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActSimpleSynthesisId,
        Constants.ActImprovisedBrewId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        // This shouldn't have anything. NPCs can't pick this class.
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "witch";
    }

    protected override void ApplyInternal()
    {
        // Crafting - 261
        owner.Chara.elements.ModPotential(261, 30);
        // Jewelry - 259
        owner.Chara.elements.ModPotential(259, 30);
        // Magic Device - 305
        owner.Chara.elements.ModPotential(305, 30);
        // Gathering - 250
        owner.Chara.elements.ModPotential(250, 30);
    }
}