using PromotionMod.Common;
using PromotionMod.Stats.WitchHunter;
namespace PromotionMod.Elements.PromotionAbilities.WitchHunter;

public class ActMagicReflect : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatWitchHunter) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.WitchHunterId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActMagicReflectId)) return false;
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
        CC.Chara.AddCondition<ConMagicReflect>();
        CC.AddCooldown(Constants.ActMagicReflectId, 5);
        return true;
    }
}