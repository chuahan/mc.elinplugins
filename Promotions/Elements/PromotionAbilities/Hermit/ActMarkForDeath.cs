using PromotionMod.Common;
using PromotionMod.Stats.Hermit;
namespace PromotionMod.Elements.PromotionAbilities.Hermit;

/// <summary>
///     Hermit Ability
///     Marks an enemy for death.
/// </summary>
public class ActMarkForDeath : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHermit) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HermitId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActMarkForDeathId)) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        TC.Chara.AddCondition<ConMarkedForDeath>();
        CC.AddCooldown(Constants.ActMarkForDeathId, 5);
        return true;
    }
}