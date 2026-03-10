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

    protected override void ApplyInternalNPC(Chara c)
    {
        // This shouldn't have anything. NPCs can't use this class.
    }

    public override string JobRequirement => "witch";
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
        // Add all the Artificer Recipes here.
        if (!EClass.player.recipes.IsKnown("artificer_workbench")) EClass.player.recipes.Add("artificer_workbench");
        if (!EClass.player.recipes.IsKnown("artificer_refined_crystal"))  EClass.player.recipes.Add("artificer_refined_crystal");
        if (!EClass.player.recipes.IsKnown("artificer_cursecube"))  EClass.player.recipes.Add("artificer_cursecube");
        if (!EClass.player.recipes.IsKnown("artificer_earthgauntlet"))  EClass.player.recipes.Add("artificer_earthgauntlet");
        if (!EClass.player.recipes.IsKnown("artificer_firesword"))  EClass.player.recipes.Add("artificer_firesword");
        if (!EClass.player.recipes.IsKnown("artificier_heavenpearl"))  EClass.player.recipes.Add("artificier_heavenpearl");
        if (!EClass.player.recipes.IsKnown("artificer_iceaxe"))  EClass.player.recipes.Add("artificer_iceaxe");
        if (!EClass.player.recipes.IsKnown("artificer_lifeflower"))  EClass.player.recipes.Add("artificer_lifeflower");
        if (!EClass.player.recipes.IsKnown("artificer_lightningspear"))  EClass.player.recipes.Add("artificer_lightningspear");
        if (!EClass.player.recipes.IsKnown("artificer_timehourglass"))  EClass.player.recipes.Add("artificer_timehourglass");
        if (!EClass.player.recipes.IsKnown("artificer_elementalbow"))  EClass.player.recipes.Add("artificer_elementalbow");
        if (!EClass.player.recipes.IsKnown("artificer_sonicbomb"))  EClass.player.recipes.Add("artificer_sonicbomb");
        if (!EClass.player.recipes.IsKnown("artificer_biobomb"))  EClass.player.recipes.Add("artificer_biobomb");
        if (!EClass.player.recipes.IsKnown("artificer_flashbomb"))  EClass.player.recipes.Add("artificer_flashbomb");
        if (!EClass.player.recipes.IsKnown("artificer_golem_core"))  EClass.player.recipes.Add("artificer_golem_core");
        if (!EClass.player.recipes.IsKnown("artificer_golem_limbs"))  EClass.player.recipes.Add("artificer_golem_limbs");
        if (!EClass.player.recipes.IsKnown("artificer_golem_flight"))  EClass.player.recipes.Add("artificer_golem_flight");
        if (!EClass.player.recipes.IsKnown("artificer_golem_aquatic"))  EClass.player.recipes.Add("artificer_golem_aquatic");
        if (!EClass.player.recipes.IsKnown("artificer_golem_pilot"))  EClass.player.recipes.Add("artificer_golem_pilot");
        if (!EClass.player.recipes.IsKnown("artificer_golem_frame_mim"))  EClass.player.recipes.Add("artificer_golem_frame_mim");
        if (!EClass.player.recipes.IsKnown("artificer_golem_frame_harpy"))  EClass.player.recipes.Add("artificer_golem_frame_harpy");
        if (!EClass.player.recipes.IsKnown("artificer_golem_frame_siren"))  EClass.player.recipes.Add("artificer_golem_frame_siren");
        if (!EClass.player.recipes.IsKnown("artificer_golem_frame_titan"))  EClass.player.recipes.Add("artificer_golem_frame_titan");
        if (!EClass.player.recipes.IsKnown("artificer_golem_precept_vanguard"))  EClass.player.recipes.Add("artificer_golem_precept_vanguard");
        if (!EClass.player.recipes.IsKnown("artificer_golem_precept_tower"))  EClass.player.recipes.Add("artificer_golem_precept_tower");
        if (!EClass.player.recipes.IsKnown("artificer_golem_precept_siege"))  EClass.player.recipes.Add("artificer_golem_precept_siege");
        if (!EClass.player.recipes.IsKnown("artificer_golem"))  EClass.player.recipes.Add("artificer_golem");
        if (!EClass.player.recipes.IsKnown("artificer_golem_memorychip"))  EClass.player.recipes.Add("artificer_golem_memorychip");
        if (!EClass.player.recipes.IsKnown("artificer_golem_componentchip"))  EClass.player.recipes.Add("artificer_golem_componentchip");
        if (!EClass.player.recipes.IsKnown("artificer_steamlight"))  EClass.player.recipes.Add("artificer_steamlight");
        if (!EClass.player.recipes.IsKnown("artificer_steamlight_book"))  EClass.player.recipes.Add("artificer_steamlight_book");
    }
}