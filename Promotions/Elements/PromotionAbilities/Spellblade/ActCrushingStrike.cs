using PromotionMod.Common;

namespace PromotionMod.Elements.PromotionAbilities.Spellblade;

public class ActCrushingStrike : ActMelee
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSpellblade) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SpellbladeId.lang()));
            return false;
        }
        return base.CanPerform();
    }
    
    // TODO: I kind of want this one to be SP.
    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        return base.Perform();
    }
}