using PromotionMod.Common;
using PromotionMod.Stats.Runeknight;
namespace PromotionMod.Elements.PromotionAbilities.Runeknight;

public class ActRuneEtching : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatRuneKnight;
    public override string PromotionString => Constants.RuneKnightId;
    public override int AbilityId => Constants.ActRuneEtchingId;

    public override bool CanPerformExtra(bool verbose)
    {
        if (CC.IsPC && CC.things.Find("tool_talisman") == null && verbose)
        {
            CC.Say("runeknight_etching_needcalligraphy".langGame());
            return false;
        }

        return true;
    }

    public override bool Perform()
    {
        TC.Chara.AddCondition<ConWardingRune>(GetPower(CC));
        return true;
    }
}