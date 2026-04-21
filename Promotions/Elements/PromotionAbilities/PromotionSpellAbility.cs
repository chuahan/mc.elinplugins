using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities;

/// <summary>
///     A wrapper class to help reduce duplicate code.
/// </summary>
public abstract class PromotionSpellAbility : PromotionAbility
{
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool CanPerform()
    {
        // Spell type abilities cannot be used with Silence.
        if (CC.HasCondition<ConSilence>()) return false;

        return base.CanPerform();
    }

    // Spellpower Scaling - Apply Spell Enhance to this ability.
    public override int GetPower(Card c)
    {
        int power = base.GetPower(c);
        return power * Mathf.Max(100 + c.Evalue(ENC.encSpell) - c.Evalue(SKILL.antiMagic), 1) / 100;
    }
}