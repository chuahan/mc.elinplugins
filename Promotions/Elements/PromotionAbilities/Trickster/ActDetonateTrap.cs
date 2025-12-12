using System.Linq;
using PromotionMod.Common;
using PromotionMod.Trait.Trickster;
namespace PromotionMod.Elements.PromotionAbilities.Trickster;

public class ActDetonateTrap : Ability
{
    public override int PerformDistance => 5;
    
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatTrickster) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.TricksterId.lang()));
            return false;
        }
        if (TP.Installed == null || TP.GetInstalled<TraitTricksterArcaneTrap>() == null) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            type = CostType.None,
            cost = 0
        };
    }

    public override bool Perform()
    {
        TraitTricksterArcaneTrap arcaneTrap = TP.GetInstalled<TraitTricksterArcaneTrap>();
        arcaneTrap.DetonateTrap(true);
        return true;
    }
}