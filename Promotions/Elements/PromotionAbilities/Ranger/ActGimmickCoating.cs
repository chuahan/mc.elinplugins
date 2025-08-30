using PromotionMod.Common;
using PromotionMod.Stats.Ranger;
namespace PromotionMod.Elements.PromotionAbilities.Ranger;

public class ActGimmickShot : Ability
{
    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }
    
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatRanger) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.RangerId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActGimmickCoatingId)) return false;
        return base.CanPerform();
    }
    
    public override bool Perform()
    {
        CC.AddCondition<ConGimmickCoating>(this.GetPower(CC));
        CC.AddCooldown(Constants.ActGimmickCoatingId, 5);
        return true;
    }
}