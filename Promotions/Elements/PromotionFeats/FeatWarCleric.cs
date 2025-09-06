using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// The one to show the way, whether you like it or not. The War Cleric has taken up violence as an alternative to compassion.
/// War Clerics focus on being durable enough to move to the front lines to rescue beleaguered allies or bring down the hammer on enemies.
/// They specialize in being able to provide curative support or frontline strength as needed, no matter the situation.
/// TODO (P3) Change name? Also maybe make it more of a martial class.
/// 
/// SKill - Divine Descent - Embodies yourself as the avatar of your god, massively boosting your physical attributes based on your Faith.
///     When descending, causes a massive holy 3F explosion.
///     Allies in the explosion are burst healed to full, debuffs purged.
///     Enemies in the explosion are damaged with holy damage and inflicted with Fear.
/// Skill - Sanctuary Stance - Deploys an area that reduces damage done to all it's inhabitants by 75%, both ally and hostile.
/// Skill - Divine Fist - Single target holy damage attack. On impact, shoots out 4 holy bolts at nearby characters.
///     If the character is hostile. It will act as a holy arrow.
///     If the character is friendly. It will instead heal the amount of damage it would have dealt.
/// Passive - Benevolence - War Clerics will apply any healing done to themselves to all friendlies within 3 Radius at 50% potency.
/// Passive - Wrath - Damage done is increased by Faith.
/// Passive - God Protects - When you pray, you and your allies gain Protection that absorbs damage based on piety.
///     This will activate when the PC prays with a Saint or War Cleric ally as well.
/// </summary>
public class FeatWarCleric : PromotionFeat
{
    public override string PromotionClassId => Constants.WarClericId;
    public override int PromotionClassFeatId => Constants.FeatWarCleric;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActDivineDescentId,
        Constants.ActDeploySanctuaryId,
        Constants.ActDivineFistId
    };
    
    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActDivineDescentId, 50, false);
        c.ability.Add(Constants.ActDeploySanctuaryId, 50, false);
        c.ability.Add(Constants.ActDivineFistId, 100, false);
    }
    
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "priest";
    }

    protected override void ApplyInternal()
    {
        // Martial Arts
        // Staff
        // Maces
        // Faith
        // Will
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}