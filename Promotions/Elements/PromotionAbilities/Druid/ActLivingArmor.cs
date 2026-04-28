using PromotionMod.Common;
using PromotionMod.Stats.Druid;
namespace PromotionMod.Elements.PromotionAbilities.Druid;

/// <summary>
///     Druid Ability
/// </summary>
public class ActLivingArmor : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatDruid;
    public override string PromotionString => Constants.DruidId;
    public override int AbilityId => Constants.ActLivingArmorId;

    public override bool CanPerformExtra(bool verbose)
    {
        if (TC.Chara.IsHostile(CC)) return false;
        return true;
    }

    public override bool Perform()
    {
        TC.Chara.AddCondition<ConLivingArmor>(GetPower(CC));
        return true;
    }
}