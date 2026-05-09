using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities;

/// <summary>
///     A wrapper class to help reduce duplicate code.
/// </summary>
public abstract class PromotionAbility : Ability
{

    public enum PromotionAbilityCostType
    {
        PromotionAbilityCostStamina, // Defaults to Stamina Cost.
        PromotionAbilityCostMana, // Converts to Mana Cost.
        PromotionAbilityCostNone // Converts to None Cost with 1 so it can still level.
    }

    // Needed for making the promotion ability class locked.
    public abstract int PromotionId { get; }
    public abstract string PromotionString { get; }

    public abstract int AbilityId { get; }

    public virtual PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;

    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(PromotionId))
        {
            Msg.Say("classlocked_ability".lang(PromotionString.lang()));
            return false;
        }

        return CanPerformExtra(true) && base.CanPerform();
    }

    public override bool ValidatePerform(Chara _cc, Card _tc, Point _tp)
    {
        return CanPerformExtra(true);
    }

    // Extra validations.
    public virtual bool CanPerformExtra(bool verbose)
    {
        return true;
    }

    public override Cost GetCost(Chara c)
    {
        Cost cost = base.GetCost(c);
        switch (PromotionAbilityCost)
        {
            case PromotionAbilityCostType.PromotionAbilityCostMana:
                cost.type = CostType.MP;
                break;
            case PromotionAbilityCostType.PromotionAbilityCostNone:
                cost.type = CostType.None;
                cost.cost = 1;
                break;
        }
        ;

        return cost;
    }
}