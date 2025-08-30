using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// Pen in one hand, sword in the other. The Battlemages are frontline mages tempered by the fires of war.
/// Battlemages focus on not merely being on the frontline, but becoming the frontline itself.
/// They specialize in heavy area damage, knocking enemies into disarray.
/// Passive - Magic Armor - Increases PV based on Max Mana.
/// Passive - Conspectus of Frontline - Bits you summmon are replaced with Shield bits.
/// Passive - Conspectus of War - Converts Elemental Spellbooks into Cannon or Hammer spells.
/// </summary>
public class FeatBattlemage : PromotionFeat
{
    public override string PromotionClassId => Constants.BattlemageId;
    public override int PromotionClassFeatId => Constants.FeatBattlemage;
    public override List<int> PromotionAbilities => new List<int>
    {
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "warmage";
    }

    protected override void ApplyInternal()
    {
        // Reading - 285
        owner.Chara.elements.ModPotential(285, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
        // Control Magic - 302
        owner.Chara.elements.ModPotential(302, 30);
    }
}