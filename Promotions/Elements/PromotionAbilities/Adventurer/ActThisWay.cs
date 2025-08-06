using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Adventurer;

/// <summary>
/// Adventurer Ability
/// Escape Ability: Teleports the player to the staircase up on the floor, or as close as they can to it.
/// 30 Turn cooldown.
/// </summary>
public class ActThisWay : Ability
{
    public override bool CanPerform()
    {
        // Unusable by NPCs.
        if (CC == null || !CC.IsPC) return false;
        return CC?.HasCooldown(Constants.ActThisWayId) ?? false;
    }

    public override bool Perform()
    {
        TraitStairs stairs = CC.currentZone.map.FindThing<TraitStairsUp>();
        Point exitPoint = stairs.GetExitPos();
        CC.Teleport(exitPoint.GetNearestPoint(false, false) ?? exitPoint);
        CC.AddCooldown(Constants.ActThisWayId, 30);
        return true;
    }
}