using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     A wonderous amalgamation of magic and technology. The Artificer focuses on the production of magical tools.
///     Artificers focus on the development and creation of magical tools of varied sizes and use.
///     They specialize in the creation, use, and recharging of magical devices. They also are able to produce their own
///     masterpiece: The Golem.
///     PC PROMOTION ONLY
///     They learn the recipe for the Artificer Altar, which allows them to craft artificer tools.
///     Skill - Simple Synthesis - Reloads magical devices/guns for you and your party.
///     Skill - Improvised Brew - Throw a random potion at enemy or ally.
///     Passive - Material Hunter - When Sun/Earth/Mana Crystals are mined out of walls, drops two instead.
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

    public override string JobRequirement => "witch";

    protected override void ApplyInternalNPC(Chara c)
    {
        // This shouldn't have anything. NPCs can't use this class.
    }

    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add, eleOwner, hint);
        // Add all the Artificer Recipes here.
        if (!player.recipes.IsKnown("artificer_workbench")) player.recipes.Add("artificer_workbench");
        if (!player.recipes.IsKnown("artificer_refined_crystal")) player.recipes.Add("artificer_refined_crystal");
        if (!player.recipes.IsKnown("artificer_cursecube")) player.recipes.Add("artificer_cursecube");
        if (!player.recipes.IsKnown("artificer_earthgauntlet")) player.recipes.Add("artificer_earthgauntlet");
        if (!player.recipes.IsKnown("artificer_firesword")) player.recipes.Add("artificer_firesword");
        if (!player.recipes.IsKnown("artificier_heavenpearl")) player.recipes.Add("artificier_heavenpearl");
        if (!player.recipes.IsKnown("artificer_iceaxe")) player.recipes.Add("artificer_iceaxe");
        if (!player.recipes.IsKnown("artificer_lifeflower")) player.recipes.Add("artificer_lifeflower");
        if (!player.recipes.IsKnown("artificer_lightningspear")) player.recipes.Add("artificer_lightningspear");
        if (!player.recipes.IsKnown("artificer_timehourglass")) player.recipes.Add("artificer_timehourglass");
        if (!player.recipes.IsKnown("artificer_elementalbow")) player.recipes.Add("artificer_elementalbow");
        if (!player.recipes.IsKnown("artificer_sonicbomb")) player.recipes.Add("artificer_sonicbomb");
        if (!player.recipes.IsKnown("artificer_biobomb")) player.recipes.Add("artificer_biobomb");
        if (!player.recipes.IsKnown("artificer_flashbomb")) player.recipes.Add("artificer_flashbomb");
        if (!player.recipes.IsKnown("artificer_golem_core")) player.recipes.Add("artificer_golem_core");
        if (!player.recipes.IsKnown("artificer_golem_limbs")) player.recipes.Add("artificer_golem_limbs");
        if (!player.recipes.IsKnown("artificer_golem_flight")) player.recipes.Add("artificer_golem_flight");
        if (!player.recipes.IsKnown("artificer_golem_aquatic")) player.recipes.Add("artificer_golem_aquatic");
        if (!player.recipes.IsKnown("artificer_golem_pilot")) player.recipes.Add("artificer_golem_pilot");
        if (!player.recipes.IsKnown("artificer_golem_frame_mim")) player.recipes.Add("artificer_golem_frame_mim");
        if (!player.recipes.IsKnown("artificer_golem_frame_harpy")) player.recipes.Add("artificer_golem_frame_harpy");
        if (!player.recipes.IsKnown("artificer_golem_frame_siren")) player.recipes.Add("artificer_golem_frame_siren");
        if (!player.recipes.IsKnown("artificer_golem_frame_titan")) player.recipes.Add("artificer_golem_frame_titan");
        if (!player.recipes.IsKnown("artificer_golem_precept_vanguard")) player.recipes.Add("artificer_golem_precept_vanguard");
        if (!player.recipes.IsKnown("artificer_golem_precept_tower")) player.recipes.Add("artificer_golem_precept_tower");
        if (!player.recipes.IsKnown("artificer_golem_precept_siege")) player.recipes.Add("artificer_golem_precept_siege");
        if (!player.recipes.IsKnown("artificer_golem")) player.recipes.Add("artificer_golem");
        if (!player.recipes.IsKnown("artificer_golem_memorychip")) player.recipes.Add("artificer_golem_memorychip");
        if (!player.recipes.IsKnown("artificer_golem_componentchip")) player.recipes.Add("artificer_golem_componentchip");
        if (!player.recipes.IsKnown("artificer_steamlight")) player.recipes.Add("artificer_steamlight");
        if (!player.recipes.IsKnown("artificer_steamlight_book")) player.recipes.Add("artificer_steamlight_book");
    }
}