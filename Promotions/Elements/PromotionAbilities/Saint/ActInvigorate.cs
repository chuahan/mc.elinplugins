using PromotionMod.Common;
using PromotionMod.Stats;

namespace PromotionMod.Elements;

public class ActInvigorate : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatSaint;
    public override string PromotionString => Constants.SaintId;
    public override int AbilityId => Constants.ActInvigorateId;

    public override bool Perform()
    {
        // Apply Invigoration based off of Faith.
        TC.Chara.AddCondition<ConInvigoration>(CC.Evalue(SKILL.faith));
        return true;
    }
}