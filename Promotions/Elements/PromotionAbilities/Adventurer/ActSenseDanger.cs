using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Adventurer;

/// <summary>
/// Adventurer Ability
/// Activate modified Telepathy: Reveals all traps and any hostile enemies on the map.
/// </summary>
public class ActSenseDanger : Ability
{
    public override bool CanPerform()
    {
        // Unusable by NPCs.
        if (CC == null || !CC.IsPC) return false;
        return CC?.HasCooldown(Constants.ActSenseDangerId) ?? false;
    }

    public override bool Perform()
    {
        CC.AddCondition<ConSenseDanger>();
        CC.AddCooldown(Constants.ActSenseDangerId, 10);
        return true;
    }
}