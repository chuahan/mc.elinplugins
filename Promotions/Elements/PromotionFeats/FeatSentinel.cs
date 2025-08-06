using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * The impenetrable shield and the all-piercing spear. Sentinels stand in front of their allies, shielding them from harm.
 * Sentinels focus on defenses and aggro control, but can temporarily shed defenses to greatly increase their attack power if the situation calls for it.
 * They specialize in utilizing their superior defenses to take damage in the place of their allies,
 *
 * ActShout - Applies ConTaunt to nearby enemies. 5 Turn Cooldown.
 * ActSmite - Slams an enemy with your shield, dealing % damage based on your Shield PV, stuns. Applies ConTaunt. 5 Turn Cooldown.
 * StanceShield - Increased PV and Mag Res, gain damage penalty. Will turn off StanceSpear.
 * StanceSpear - Sacrifices all your PV to gain melee damage boost based on how much PV you had. Will Turn off StanceShield.
 * Sentinel Class - Doubles your PV when wearing Shield Style + Heavy Armor.
 *                - When using Shield Style, 85% to block damage and reduce it by 50%. Gain Retaliate on block.
 *                - Debuffs expire twice as fast on Sentinels.
 */
public class FeatSentinel : PromotionFeat
{
    public override string PromotionClassId => Constants.SentinelId;
    public override int PromotionClassFeatId => Constants.FeatSentinel;
    public override List<int> PromotionAbilities => new List<int>
    {
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "warrior";
    }
    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}