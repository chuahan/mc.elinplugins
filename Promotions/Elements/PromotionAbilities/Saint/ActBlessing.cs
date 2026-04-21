using PromotionMod.Common;
using PromotionMod.Stats.Saint;
namespace PromotionMod.Elements.PromotionAbilities.Saint;

public class ActBlessing : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatSaint;
    public override string PromotionString => Constants.SaintId;

    public override int AbilityId => Constants.ActBlessingId;
    public override bool Perform()
    {
        // Apply Blessing based off of BASE Faith.
        TC.Chara.AddCondition<ConBlessing>(CC.elements.Base(SKILL.faith));
        return true;
    }
}