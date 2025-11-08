using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.Maia;

/// <summary>
/// Increases Speed, Melee Attack, and Accuracy
/// </summary>
public class ActEnlightenedEmpowerment : Ability
{
    public override bool CanPerform()
    {
        // Ability is only usable by Enlightened Maia.
        if (CC.Evalue(Constants.FeatMaia) == 0 || CC.Evalue(Constants.FeatMaiaEnlightened) == 0)
        {
            return false;
        }
        return base.CanPerform();
    }
}