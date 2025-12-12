using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The impenetrable shield and the all-piercing spear. Sentinels stand in front of their allies, shielding them from
///     harm.
///     Sentinels focus on defenses and aggro control, but can temporarily shed defenses to greatly increase their attack
///     power if the situation calls for it.
///     They specialize in utilizing their superior defenses to take damage in the place of their allies,
///     Skill - Shout - Applies ConTaunt to nearby enemies. 5 Turn Cooldown.
///     Skill - Shield Smite - Slams an enemy with your shield, dealing % damage based on your Shield PV, stuns. Applies
///     ConTaunt. 5 Turn Cooldown.
///     Skill - Restraint Stance - Increased PV and Mag Res, gain damage penalty.
///     Skill - Rage Stance - Snapshots your PV.
///     You gain a debuff that reduces your PV by that amount.
///     Your melee attacks gain increased damage based on your PV lost.
///     You cannot activate Sentinel's Stance while this Buff is active.
///     Class Passive - Sentinel - Increases PV by 50% for each Heavy Armor and Shield.
///     Passive - Block - When using Shield Style, 85% to block damage and reduce it between 25 to 75%. Gain Retaliate on
///     block.
///     Retaliate - Your next melee attack will do increased damage.
/// </summary>
public class FeatSentinel : PromotionFeat
{
    public override string PromotionClassId => Constants.SentinelId;
    public override int PromotionClassFeatId => Constants.FeatSentinel;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActShoutId,
        Constants.ActShieldSmiteId,
        Constants.StRageId,
        Constants.StRestraintId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActShoutId, 100, false);
        c.ability.Add(Constants.ActShieldSmiteId, 75, false);
        c.ability.Add(Constants.StRestraintId, 100, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "warrior";
    }
    
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
    
    public static long GetShieldPower(Chara cc)
    {
        int totalShield = 0;
        foreach (BodySlot slot in cc.body.slots)
        {
            if (slot.elementId == 35 && slot.thing != null && !slot.thing.IsMeleeWeapon)
            {
                totalShield += slot.thing.PV;
            }
        }

        return totalShield;
    }
}