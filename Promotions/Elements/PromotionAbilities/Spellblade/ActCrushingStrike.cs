using PromotionMod.Common;
using PromotionMod.Stats.Spellblade;
namespace PromotionMod.Elements.PromotionAbilities.Spellblade;

public class ActCrushingStrike : Ability
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

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        CC.AddCondition<ConCrushingStrikeAttack>(this.GetPower(CC), true);
        return new ActMelee().Perform(CC, TC);
    }
}