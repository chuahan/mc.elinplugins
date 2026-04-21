using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Machinist;
namespace PromotionMod.Elements.PromotionAbilities.Machinist;

public class ActOverclock : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatMachinist;
    public override string PromotionString => Constants.MachinistId;
    public override int Cooldown => 10;
    public override int AbilityId => Constants.ActOverclockId;

    public override bool CanPerformExtra()
    {
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
        int power = GetPower(CC);
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, true).Where(target => target.HasTag(CTAG.machine)))
        {
            target.AddCondition<ConOverclock>(power);
        }
        CC.AddCondition<ConOverclock>(power);

        CC.AddCooldown(AbilityId, Cooldown);
        return true;
    }
}