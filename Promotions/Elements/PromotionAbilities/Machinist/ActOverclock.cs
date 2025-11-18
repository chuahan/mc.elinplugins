using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Machinist;
namespace PromotionMod.Elements.PromotionAbilities.Machinist;

public class ActOverclock : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatMachinist) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.MachinistId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActOverclockId)) return false;
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
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, true).Where(target => target.HasTag(CTAG.machine)))
        {
            target.AddCondition<ConOverclock>();
        }

        CC.AddCooldown(Constants.ActOverclockId, 10);
        return true;
    }
}