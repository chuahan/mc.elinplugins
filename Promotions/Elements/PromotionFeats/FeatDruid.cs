using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The speaker for nature. Druids are able to call forth vitality even in the most barren places.
///     Druids focus on magics related to nature, summoning plantlife to aid their cause.
///     They specialize in supporting their teams through casting various control spells.
///     Skill - Sow Wrath Seeds - Summon Offensive flowers
///     Skill - Sow Warm Seeds - Summon Defensive or Support flowers
///     Skill - Living Armor - Surrounds the target with living vines, increasing their PV and providing regeneration.
///     Spell - Summon Tree Ent Warrior
///     Passive - Conspectus of Nature - Convert Summon books to Summon Tree Ent Warrior
///     Passive - Friends with Nature. Wild plants and animals become friendly.
/// </summary>
public class FeatDruid : PromotionFeat
{
    public override string PromotionClassId => Constants.DruidId;
    public override int PromotionClassFeatId => Constants.FeatDruid;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActSowWarmSeedsId,
        Constants.ActSowWrathSeedsId,
        Constants.ActLivingArmorId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActSowWarmSeedsId, 60, false);
        c.ability.Add(Constants.ActSowWrathSeedsId, 60, false);
        c.ability.Add(Constants.SpSummonTreeEntId, 80, false);
        c.ability.Add(Constants.ActLivingArmorId, 80, true);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "farmer";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
}