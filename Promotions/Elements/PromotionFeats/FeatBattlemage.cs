using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     Pen in one hand, sword in the other. The Battlemages are frontline mages tempered by the fires of war.
///     Battlemages focus on not merely being on the frontline, but becoming the frontline itself.
///     They specialize in heavy area damage, knocking enemies into disarray.
///     Skill - Mana Shield Stance
///     At the cost of reserving your mana (-25% max mana), you will gain a regenerating shield that will take damage
///     before your HP does.
///     When you haven't taken damage for a time, it will regenerate at 5% a turn.
///     Shield Capacity is based off of mana reserved.
///     Skill - Mana Focus Stance  - Gain Spellpower based on current mana. Will not do anything if the Battlemage is at 0
///     or negative mana.
///     When entering this stance, the Battlemage will gain additional spellpower based off of 15% of their current mana.
///     Their spells will also cost more, increasing costs by 5% of their current mana.
///     Their spells will pierce 1 tier of elemental resistances.
///     Passive - Conspectus of War - Can convert Elemental Spellbooks into Flare of the same element. // TODO Flare turns out not to be fully implemented.
///     Passive - Magic Armor - Increases PV based on Max Mana.
///     Passive - Shield Bit Conversion - Bits summoned are replaced with Shield bits.
/// </summary>
public class FeatBattlemage : PromotionFeat
{
    public override string PromotionClassId => Constants.BattlemageId;
    public override int PromotionClassFeatId => Constants.FeatBattlemage;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.StManaShieldId,
        Constants.StManaFocusId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.StManaShieldId, 100, false);
        c.ability.Add(Constants.StManaFocusId, 100, false);
        c.ability.Add(50611, 50, false); // Magic Bits
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "warmage";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
}