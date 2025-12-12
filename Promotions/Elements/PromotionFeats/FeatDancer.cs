using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     Grace in motion. The Dancer specializes in inspiring allies and bewitching foes.
///     Dancers focus on supporting allies with their beautiful dancing, while mixing in attacks during the dance to assail
///     enemies.
///     They specialize in supporting all or a single melee oriented ally while doing light damage and statuses to enemies.
///     Skill - Dance Partner - Selects a Dance Partner. Buffs you apply via dancing will apply to only you and your
///     partner, but be enhanced.
///     Blade Dancing Skills - All of these can only be used while one of the
///     Skill - Dagger Illusion - 5 Radius attack. Throw 5 knives at nearby enemies,and randomly one of the three: bleed,
///     paralysis, and poison.
///     Skill - Sword Fouette - 2 Radius Swarm. For each enemy hit it applies a damage reduction to your next blow.
///     Skill - Wild Pirouette - Debuffs nearby targets with various debuffs. Charm/Sleep/Jealousy
///     Dance Stances - Only one dance can be active at once.
///     Skill - Energy Dance Stance - Reduces Cost of abilities
///     Skill - Freedom Dance Stance - Removes a debuff each turn.
///     Skill - Healing Dance Stance - Heals allies 10% every turn.
///     Skill - Mist Dance Stance - Increases DV and PV.
///     Skill - Swift Dance Stance - Increases Speed.
/// </summary>
public class FeatDancer : PromotionFeat
{
    public override string PromotionClassId => Constants.DancerId;
    public override int PromotionClassFeatId => Constants.FeatDancer;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActDancePartnerId,
        Constants.ActDaggerIllusionId,
        Constants.ActSwordFouetteId,
        Constants.ActWildPirouetteId,
        Constants.StEnergyDanceId,
        Constants.StFreedomDanceId,
        Constants.StHealingDanceId,
        Constants.StMistDanceId,
        Constants.StSwiftDanceId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActDaggerIllusionId, 35, false);
        c.ability.Add(Constants.ActSwordFouetteId, 35, false);
        c.ability.Add(Constants.ActWildPirouetteId, 35, false);

        c.ability.Add(Constants.StEnergyDanceId, 25, false);
        c.ability.Add(Constants.StFreedomDanceId, 25, false);
        c.ability.Add(Constants.StHealingDanceId, 25, false);
        c.ability.Add(Constants.StSwiftDanceId, 25, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "pianist";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
}