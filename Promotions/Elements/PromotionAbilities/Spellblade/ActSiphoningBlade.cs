using PromotionMod.Common;
using PromotionMod.Stats.Spellblade;

namespace PromotionMod.Elements.PromotionAbilities.Spellblade;

public class ActSiphoningBlade : ActMelee
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSpellblade) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SpellbladeId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActSiphoningBladeId)) return false;
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
        // Add Siphoning Blade. This gets hooked later to convert the damage into mana damage instead.
        // Kill it right after completion.
        TC.PlayEffect("curse");
        Condition siphon = CC.AddCondition<ConSiphoningBlade>();
        Attack();
        CC.AddCooldown(Constants.ActSiphoningBladeId, 5);
        siphon.Kill();
        return true;
    }
}