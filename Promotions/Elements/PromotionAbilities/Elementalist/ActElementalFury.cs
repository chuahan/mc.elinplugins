using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Elementalist;

/// <summary>
///     Elementalist Ability
///     Consumes all orbs and apply Elemental Storm to yourself based on how many elements you had.
/// </summary>
public class ActElementalFury : PromotionSpellAbility
{

    public int ElementalFuryRequirement = 4;
    public override int PromotionId => Constants.FeatElementalist;
    public override string PromotionString => Constants.ElementalistId;
    public override int AbilityId => Constants.ActElementalFuryId;

    public override bool CanPerformExtra()
    {
        if (!CC.MatchesPromotion(Constants.FeatElementalist))
        {
            Msg.Say("classlocked_ability".lang(Constants.ElementalistId.lang()));
            return false;
        }
        if (CC.HasCondition<ConElementalist>())
        {
            ConElementalist elementalist = CC.GetCondition<ConElementalist>();
            if (elementalist.GetElementalCombination() < ElementalFuryRequirement)
            {
                Msg.Say("elementalist_notenoughorbs".langGame(ElementalFuryRequirement.ToString()));
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public override bool Perform()
    {
        ConElementalist elementalist = CC.GetCondition<ConElementalist>();
        // Clone the Elemental Stockpile to deplete.
        ConElementalFury fury = CC.AddCondition<ConElementalFury>(GetPower(CC)) as ConElementalFury;
        fury.ElementalStockpile = new Dictionary<int, int>(elementalist.ElementalStockpile);

        int orbsConsumed = elementalist.ElementalStockpile.Values.Sum(); 
        int spellExp = CC.elements.GetSpellExp(CC, this.act, 100) * orbsConsumed;
        CC.ModExp(this.AbilityId, spellExp);
        
        elementalist.ConsumeElementalOrbs();
        return true;
    }
}