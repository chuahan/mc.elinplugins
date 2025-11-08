using PromotionMod.Common;
using PromotionMod.Stats.Adventurer;
namespace PromotionMod.Elements.PromotionAbilities.Adventurer;

public class ActSenseDanger : Ability
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
        if (CC.HasCooldown(Constants.ActSenseDangerId)) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        CC.AddCondition<ConSenseDanger>();
        CC.AddCooldown(Constants.ActSenseDangerId, 10);
        return true;
    }
}