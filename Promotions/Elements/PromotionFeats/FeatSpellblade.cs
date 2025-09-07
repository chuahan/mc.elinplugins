using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The blade with magical flair. The Spellblade strikes swiftly with magic imbued weapons.
///     Spellblades focus on exploiting enemy weaknesses and crippling the enemy.
///     They specialize in reducing enemy resistances and applying status effects, as well as magical swords.
///     Skill - Crushing Strike - Executes a melee attack targeting a random body part of the enemy, applying a different
///     debuff.
///     Head - Blind and Faint
///     Neck - Silence
///     Torso/Back/Waist - PV Break.
///     Arm/Hand/Finger - Disable
///     Feet/Legs - Speed Break.
///     Skill - Myriad Fleche - Executes a thrusting melee attack, closing in an enemy.
///     On impact, fires a piercing breath attack in the direction of the target that pierces enemy resistances.
///     If the user is currently affected by an intonation, that element is used.
///     If no intonation is active, a random element between Fire, Lightning, Ice, and Poison is chosen.
///     Any enemy struck by the followup wave loses resistances to that element.
///     Skill - Siphoning Blade - Executes an attack that targets Mana instead of HP, absorbing half the "damage" done as
///     MP.
///     Passive - Conspectus of the Blade - Can convert Elemental Spellbooks into Magic Sword.
///     Passive - Elemental Mastery - Excel at status application. EleP is doubled.
/// </summary>
public class FeatSpellblade : PromotionFeat
{
    public override string PromotionClassId => Constants.SpellbladeId;
    public override int PromotionClassFeatId => Constants.FeatSpellblade;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActCrushingStrikeId,
        Constants.ActMyriadFlecheId,
        Constants.ActSiphoningBladeId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActCrushingStrikeId, 80, false);
        c.ability.Add(Constants.ActMyriadFlecheId, 80, false);
        c.ability.Add(Constants.ActSiphoningBladeId, 50, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "warmage";
    }

    protected override void ApplyInternal()
    {
        // Sword - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}