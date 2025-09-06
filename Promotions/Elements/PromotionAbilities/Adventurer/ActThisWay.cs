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
        if (CC.Evalue(Constants.FeatAdventurer) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.AdventurerId.lang()));
            return false;
        }
        // Unusable by NPCs.
        if (!CC.IsPC) return false;
        if (CC.HasCooldown(Constants.ActThisWayId)) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        TraitStairs stairs = CC.currentZone.map.FindThing<TraitStairsUp>();
        Point exitPoint = stairs.owner.pos;
        CC.Teleport(exitPoint.GetNearestPoint(false, false) ?? exitPoint);
        CC.AddCooldown(Constants.ActThisWayId, 30);
        return true;
    }
}