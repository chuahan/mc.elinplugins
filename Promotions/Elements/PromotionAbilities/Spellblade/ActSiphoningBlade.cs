using PromotionMod.Common;
using PromotionMod.Stats.Spellblade;
namespace PromotionMod.Elements.PromotionAbilities.Spellblade;

public class ActSiphoningBlade : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatSpellblade;
    public override string PromotionString => Constants.SpellbladeId;
    public override int AbilityId => Constants.ActSiphoningBladeId;
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool Perform()
    {
        // Add Siphoning Blade. This gets hooked later to convert the damage into mana damage instead.
        // Kill it right after completion.
        TC.PlayEffect("curse");
        Condition siphon = CC.AddCondition<ConSiphoningBlade>();
        new ActMelee().Perform(CC, TC);
        siphon.Kill();
        return true;
    }
}