using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// Light our way! The Saint brings down the light of judgement upon their enemy.
/// Saint focus on healing allies while casting holy magic at the enemies.
/// They specialize in casting advanced curative magic and applying disabling debuffs, as well as empowered Holy Magic.
/// Passive - God Protects - When you pray, you and your allies gain a shield that absorbs damage based on piety. When NPCs pray
/// 
/// </summary>
public class FeatSaint : PromotionFeat
{
    public override string PromotionClassId => Constants.SaintId;
    public override int PromotionClassFeatId => Constants.FeatSaint;
    public override List<int> PromotionAbilities => new List<int>
    {
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "priest";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}