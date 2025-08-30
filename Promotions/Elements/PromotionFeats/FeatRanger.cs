using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// The guardian of the woodlands. Rangers are survivalists who favor the forests and the wildlife opposed to civilization.
/// Rangers focus on mobility and survivability, utilizing traps and light feet to keep themselves out of harms way.
/// They specialize in debuffing enemies and laying down covering fire from the backlines.
/// Skill - Ranger's Canto - When activated, while the Ranger is mounted (either is riding, or is riding as a parasite)
///         the Ranger will automatically fire at hostile targets when moving, bypassing the mounted action prevention.
///         This will not bypass reload.
/// Skill - Gimmick Shot - Loads up one of 5 gimmick shots. Ranged attacks can apply specific statuses. Cooldown 10 turns.
///         Shatter Shot: After striking the target, the projectile will shatter and target up to two additional hostile targets.
///         Hammer Shot: The projectile will attempt to apply dim.
///         Serrated Shot: The projectile will attempt to apply bleed.
///         Venom Shot: The projectile will attempt to apply poison.
///         Paralytic Shot: The projectile will attempt to apply paralysis.
/// Skill - Throw Trap - Set a trap at the target location.
/// </summary>
public class FeatRanger : PromotionFeat
{
    public override string PromotionClassId => Constants.RangerId;
    public override int PromotionClassFeatId => Constants.FeatRanger;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.StanceRangersCantoId,
        Constants.ActGimmickCoatingId,
        Constants.ActSetTrapId,
    };

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "archer";
    }

    protected override void ApplyInternal()
    {
        // Bows - 286
        // Crossbows
        owner.Chara.elements.ModPotential(286, 30);
        // Marksmanship - 304
        owner.Chara.elements.ModPotential(304, 30);
        // Riding
    }
}