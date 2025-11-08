using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.Maia;

/// <summary>
/// Increases HP.
/// </summary>
public class ActCorruptedEmpowerment : Ability
{
    public override bool CanPerform()
    {
        // Ability is only usable by corrupted Maia.
        if (CC.Evalue(Constants.FeatMaia) == 0 || (CC.Evalue(Constants.FeatMaiaCorrupted) == 0))
        {
            return false;
        }
        return base.CanPerform();
    }
}