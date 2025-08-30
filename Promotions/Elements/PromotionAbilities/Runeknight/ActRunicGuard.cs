using PromotionMod.Common;
using PromotionMod.Stats.Runeknight;

namespace PromotionMod.Elements.PromotionAbilities.Runeknight;

public class ActRunicGuard : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatRuneknight) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.RuneknightId.lang()));
            return false;
        }

        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        // Remove Existing Elemental Attunement.
        ConElementalAttunement existingAttunement = CC.GetCondition<ConElementalAttunement>();
        existingAttunement?.Kill();

        // Apply Runic Guard.
        CC.AddCondition<ConRunicGuard>();
        return true;
    }
}