using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Saint;

public class ActHandOfGod : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSaint) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SaintId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActHandOfGodId)) return false;
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
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, false))
        {
            int power = (int)HelperFunctions.SigmoidScaling(CC.Evalue(Constants.FaithId), .25F, 5F);
            power += GetPower(CC);
            TC.HealHP(power, HealSource.Magic);
            TC.Chara.AddCondition<ConGreaterRegen>(CC.Evalue(Constants.FaithId));
        }

        CC.AddCooldown(Constants.ActHandOfGodId, 30);
        return true;
    }
}