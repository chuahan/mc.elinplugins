using PromotionMod.Common;
using PromotionMod.Stats.Sentinel;
namespace PromotionMod.Elements.PromotionAbilities.Sentinel;

public class ActShout : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSentinel) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SentinelId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActShoutId)) return false;
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
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true))
        {
            ConTaunted taunted = target.AddCondition<ConTaunted>(force: true) as ConTaunted;
            taunted.TaunterUID = CC.uid;
        }

        CC.AddCooldown(Constants.ActShoutId, 5);
        CC.TalkRaw("sentinelTaunt".langList().RandomItem());
        return true;
    }
}