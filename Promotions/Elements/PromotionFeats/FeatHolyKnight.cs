using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The Holy Knight is a defensive Paladin promotion that rushes the enemy with a host of Holy attacks.
///     Boasting impressive defenses, they can deflect attacks while rushing down the enemy, making space between allies and their enemies.
///
///     TODO: IMPLEMENT
/// 
///     Skill - Vanguard Stance - A stance that redirects all damage done to nearby non-summon allies to you. Basically a
///     stance wall of flesh.
///     Skill - Spearhead Charge - Charges through all enemies to a specific point. Knocks them away. For every enemy in the
///     path, does damage and summons a Holy Sword Bit.
///     Skill - Deflection - Enter a Deflection state for one turn. If you take damage while you are Deflecting:
///     Reduce damage by 100%.
///     Reduces the cooldown of Deflection to 0 (Recharges it instantly)
///     Summons a Holy Sword Bit.
///
///     Skill - Holy Banner - Summons a Holy Banner minion that has 10HP on the target tile. This character cannot move or attack.
///         Every turn, if the Holy Knight is neighboring the Banner, it provide area of effects.
///         Any Enemies within range will be hit with Holy Damage with a chance to stun. This power is doubled if they are Undead.
///         Any allies within range (Including the Holy Knight) will be granted Protection.
/// 
///     Passive - Heavenly Host - Every time you summon a Holy Sword Bit, you will gain a stack of 2% damage reduction capping at 10 stacks.
///     Passive - Aegis - If you have a shield equipped, chance to reduce incoming damage by 50%.
///         Chance is based off of Shield Skill. Caps at 60%.
///
///     TODO: Rename to Holy Knight
///         Vanguard Remains the Same
///         Light Wave -> Spearhead
///         Rename Parry -> Deflection
///         Add Sol -> Add condition to heal 30% damage as life with melee attacks
///         Add Aegis as a passive - Reduce incoming damage by 50% with a chance.
/// </summary>
public class FeatHolyKnight : PromotionFeat
{
    public override string PromotionClassId => Constants.HolyKnightId;
    public override int PromotionClassFeatId => Constants.FeatHolyKnight;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.StVanguardId,
        Constants.ActSpearheadId,
        Constants.ActDeflectionId,
        Constants.ActBlessedArmamentsId,
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.StVanguardId, 100, false);
        c.ability.Add(Constants.ActSpearheadId, 75, false);
        c.ability.Add(Constants.ActDeflectionId, 75, false);
        c.ability.Add(Constants.ActDeflectionId, 25, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "paladin";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
}