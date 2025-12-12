using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     Light our way! The Saint brings down the light of judgment upon their enemy.
///     Saints focus on healing allies while casting holy magic at the enemies.
///     They specialize in casting advanced curative magic and applying disabling debuffs, as well as empowered Holy Magic.
///     Skill - Hand of God - A large scale healing spell is cast on all allies and minions, healing 35% HP immediately.
///     All affected characters also gain a % healing over time.
///     30 Turn Cooldown.
///     Skill - Blessing - Applies a buff to the target that boosts their faith. Up to 50% boost scaling off your own
///     faith.
///     Passive - God Protects - When you pray, you and your allies gain Protection that absorbs damage based on piety.
///     This will activate when the PC prays with a Saint or War Cleric ally as well.
///     Passive - In their name - When the Saint inflicts damage against an enemy that matches their religion,
///     if the Saint's Piety is higher than the target, it will convert the target into a temporary ally.
///     Passive - Conspectus of Light - Can convert Attack spellbooks into Holy Element.
/// </summary>
public class FeatSaint : PromotionFeat
{
    public override string PromotionClassId => Constants.SaintId;
    public override int PromotionClassFeatId => Constants.FeatSaint;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActHandOfGodId,
        Constants.ActBlessingId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActHandOfGodId, 75, false);
        c.ability.Add(Constants.ActBlessingId, 75, true);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "priest";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
}