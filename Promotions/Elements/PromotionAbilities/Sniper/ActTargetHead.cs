using PromotionMod.Common;
using PromotionMod.Stats.Sniper;

namespace PromotionMod.Elements.PromotionAbilities.Sniper;

public class ActTargetHead : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSniper) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SniperId.lang()));
            return false;
        }

        return base.CanPerform() && ACT.Ranged.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        ConSniperTarget sniperTarget = CC.AddCondition<ConSniperTarget>(this.GetPower(CC)) as ConSniperTarget;
        sniperTarget.Target = ConSniperTarget.TargetPart.Head;
        // Perform a Ranged attack at the target.
        ACT.Ranged.Perform(CC, TC);
        sniperTarget.Kill();
        return true;
    }
}