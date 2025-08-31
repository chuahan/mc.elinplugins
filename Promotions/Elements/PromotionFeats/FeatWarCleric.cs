using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// The one to show the way, whether you like it or not. The War Cleric has taken up violence as an alternative to compassion.
/// War Clerics focus on being durable enough to move to the front lines to rescue beleaguered allies or bring down the hammer on enemies.
/// They specialize in being able to provide curative support or frontline strength as needed, no matter the situation.
/// TODO: Change to Holy Knight?
/// SKill - Divine Descent - Embodies yourself as the avatar of your god, massively boosting your stats.
/// Passive - Smite in the name of god - Damage done is increased by Piety.
/// Passive or Stance? - Turn Undead - Depending on the god you worship, nearby enemies of X tag take passive damage and are inflicted with fear/weakness.
/// </summary>
public class FeatWarCleric : PromotionFeat
{
    public override string PromotionClassId => Constants.WarClericId;
    public override int PromotionClassFeatId => Constants.FeatWarCleric;
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