using PromotionMod.Common;
using PromotionMod.Stats.Jenei;
using PromotionMod.Stats.WitchHunter;
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
        // Spell type abilities cannot be used with Silence or Null Presence.
        if (CC.HasCondition<ConSilence>()) return false;
        if (CC.HasCondition<ConNullPresence>()) return false;

        return base.CanPerform();
    }

    // Spellpower Scaling - Apply Spell Enhance to this ability.
    public override int GetPower(Card c)
    {
        int power = base.GetPower(c);
        return power * Mathf.Max(100 + c.Evalue(ENC.encSpell) - c.Evalue(SKILL.antiMagic), 1) / 100;
    }
    
    // In the Spell Ability, this is where the Jenei orb increase will be tracked for their Adept Abilities.
    public override bool ValidatePerform(Chara _cc, Card _tc, Point _tp)
    {
        bool validateResult = CanPerformExtra(true);

        if (!validateResult) return validateResult;
        
        // Jenei - Track Spellcasts for Impact/Fire/Cold/Lightning off their own abilities
        if (_cc.HasElement(Constants.FeatJenei))
        {
            string spellType = this.act.source.aliasRef;
            if (Constants.ElementAliasLookup.ContainsValue(spellType))
            {
                int element = Constants.ElementIdLookup[spellType];
                if (element is Constants.EleImpact or Constants.EleFire or Constants.EleCold or Constants.EleLightning)
                {
                    ConJenei? jenei = _cc.GetCondition<ConJenei>() ?? _cc.AddCondition<ConJenei>() as ConJenei;
                    jenei?.AddElement(element);
                }
            }
        }

        return validateResult;
    }
}