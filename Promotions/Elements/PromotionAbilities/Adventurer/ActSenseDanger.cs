using PromotionMod.Common;
using PromotionMod.Stats.Adventurer;
namespace PromotionMod.Elements.PromotionAbilities.Adventurer;

public class ActSenseDanger : PromotionAbility
{
    public override int PromotionId => Constants.FeatAdventurer;
    public override string PromotionString => Constants.AdventurerId;
    public override int Cooldown => 10;

    public override int AbilityId => Constants.ActSenseDangerId;

    public override bool CanPerformExtra()
    {
        return CC.IsPC;
    }

    public override bool Perform()
    {
        CC.AddCondition<ConSenseDanger>();
        CC.AddCooldown(AbilityId, Cooldown);
        return true;
    }
}