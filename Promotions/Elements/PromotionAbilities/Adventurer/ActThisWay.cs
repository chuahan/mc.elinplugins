using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Adventurer;

/// <summary>
///     Adventurer Ability
///     Escape Ability: Teleports the player to the staircase up on the floor, or as close as they can to it.
///     30 Turn cooldown.
/// </summary>
public class ActThisWay : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatAdventurer;
    public override string PromotionString => Constants.AdventurerId;
    public override int AbilityId => Constants.ActThisWayId;
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool CanPerformExtra(bool verbose)
    {
        // Unusable by NPCs.
        if (!CC.IsPC) return false;

        // Not usable outside of dungeons.
        if (CC.currentZone.IsPCFactionOrTent || CC.currentZone is not Zone_Dungeon)
        {
            if (CC.IsPC && verbose) Msg.Say("adventurer_thisway_dungeononly".langGame());
            return false;
        }

        return true;
    }

    public override bool Perform()
    {
        TraitStairs stairs = CC.currentZone.map.FindThing<TraitStairsUp>();
        if (stairs == null)
        {
            CC.SayNothingHappans();
            return false;
        }
        Point exitPoint = stairs.owner.pos;
        CC.Teleport(exitPoint.GetNearestPoint(false, false) ?? exitPoint);
        return true;
    }
}