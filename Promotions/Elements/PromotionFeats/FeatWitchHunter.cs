using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     Suffer not the heretic. Witch Hunters have decided they really hate mages, and will go through any means to see
///     them gone.
///     Witch Hunters focus on hunting down casters with their anti-magic in combat.
///     They specialize in being the absolute nightmare of any mage on the field through massive spell resistances and
///     anti-casting abilities.
///     Skill - Mana Break - Causes a Magic Damage Explosion that deals damage based on how much mana the target is
///     missing.
///     Skill - Magic Reflect - Activates reflective magical shield that redirects an incoming magical spell at the caster.
///     Skill - Null Zone - The caster gains Null Zone for 10 turns, 10 turn cooldown.
///     All characters within 5F of the Null Zone are afflicted with Spell Null, which will set cast chance to 0% for any
///     spell.
///     30 Turn Cooldown
///     Passive - Bane of Magickind
///     In additon to Excommunication, Bane inflicts Silence.
///     When the Witch Hunter inflicts damage, 10% of the damage done is mana damage. TODO: implement.
/// </summary>
public class FeatWitchHunter : PromotionFeat
{
    public override string PromotionClassId => Constants.WitchHunterId;
    public override int PromotionClassFeatId => Constants.FeatWitchHunter;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActManaBreakId,
        Constants.ActMagicReflectId,
        Constants.ActNullZoneId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActManaBreakId, 20, false);
        c.ability.Add(Constants.ActMagicReflectId, 85, false);
        c.ability.Add(Constants.ActNullZoneId, 50, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "inquisitor";
    }

    protected override void ApplyInternal()
    {
        // Gun Skill - 105
        owner.Chara.elements.ModPotential(105, 30);
        // Crossbow Skill - 109
        owner.Chara.elements.ModPotential(109, 30);
        // Sword - 101
        owner.Chara.elements.ModPotential(101, 30);
        // Will - 75
        owner.Chara.elements.ModPotential(75, 10);
        // Base Antimagic - 93
        owner.Chara.elements.ModBase(93, 30);
    }
}