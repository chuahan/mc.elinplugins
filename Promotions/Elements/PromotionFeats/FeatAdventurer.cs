using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// Seeker of new lands. The Adventurer lives for the unknown, highly adaptable to any situations.
/// Adventurers focus on exploration, not exactly well focused on any specific type of combat.
/// They specialize in non-combat support abilities.
/// PC PROMOTION ONLY
/// Skill - This Way - Teleports the player to the staircase up on the floor,or as close as they can to it.
///     30 Turn Cooldown.
/// Skill - Sense Danger - Adds advanced telepathy, highlights any hostile enemy and all traps.
///     10 Turn Cooldown.
/// Passive - Loot Goblin - Chance of double loot drop.
/// Passives - Leadership - Increases EXP gain for you and your allies.
/// Passive - Auto Medicate - If a party member gains a debuff that is curable with a potion, Adventurers will automatically use the potion on them. 50% chance to save the potion.
/// </summary>
public class FeatAdventurer : PromotionFeat
{
    public override string PromotionClassId => Constants.AdventurerId;
    public override int PromotionClassFeatId => Constants.FeatAdventurer;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActThisWayId,
        Constants.ActSenseDangerId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        // This shouldn't have anything. NPCs can't pick this class.
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "tourist";
    }

    protected override void ApplyInternal()
    {
        // Lol.
    }
}