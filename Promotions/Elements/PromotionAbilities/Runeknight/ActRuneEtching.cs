using PromotionMod.Common;
using PromotionMod.Stats.Runeknight;
namespace PromotionMod.Elements.PromotionAbilities.Runeknight;

public class ActRuneEtching : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatRuneKnight) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.RuneKnightId.lang()));
            return false;
        }

        if (CC.IsPC && CC.things.Find("tool_talisman") == null)
        {
            CC.Say("hint_runeknight_need_calligraphy");
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
        TC.Chara.AddCondition<ConWardingRune>(GetPower(CC));
        return true;
    }
}