using PromotionMod.Common;
using PromotionMod.Stats.Adventurer;
namespace PromotionMod.Elements.PromotionAbilities.Adventurer;

public class ActSenseDanger : PromotionAbility
{
    public override int PromotionId => Constants.FeatAdventurer;
    public override string PromotionString => Constants.AdventurerId;
    public override int AbilityId => Constants.ActSenseDangerId;
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool CanPerformExtra(bool verbose)
    {
        return CC.IsPC;
    }

    public override bool Perform()
    {
        CC.AddCondition<ConSenseDanger>();
        return true;
    }
}