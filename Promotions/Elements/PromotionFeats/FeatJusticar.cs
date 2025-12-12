using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The iron fist of judgement. To see a Justicar is to see fear, and to pity the one who broke it.
///     Justicars focus on shifting momentum through intimidation and execution.
///     They specialize in singling out targets and bringing them down, and breaking enemy morale in the process.
///
///     Justicars will balance between Law (26 to 100) / Neutrality (-25 to 25) / Chaos (-26 to -100)
///     Based on their alignment, their abilities will perform differently.
///         Lawful Justicars tend to be more supportive.
///         Chaos Justicars strike fear into the hearts of all.
///         Neutral Justicars will randomly apply one of the two effects.
/// 
///     Ability - Intimidate - Inflicts Armor break on the target. Also inflicts excommunicate. Inflicts fear on other
///     enemies near the target.
///         Law - Also inflicts armor boost on nearby allies.
///         Chaos - 
///     Ability - Subdue - Inflicts Suppress, and Attack Break on the target. Also inflicts excommunicate.
///         Law - 
///         Chaos - 
///     Ability - Condemn - Inflicts Entangle on nearby enemies. For every enemy impacted, Justicar grants their team
///     Protection.
///         Law - 
///         Chaos - 
///     Flames of Judgement - Justicars will reserve a part of their mana to activate a flaming aura around themselves.
///     Justicar gains Regeneration that scales with the spellpower.
///     Every turn, the Justicar will take 30% of their maximum health as fire damage. This same damage is also dealt to
///     nearby enemies.
///     Fire Resistance affects both outgoing and self-damage.
///         Law - Heals allies in the range for half the outgoing damage.
///         Chaos - Inflicts Fire Break on enemies.
/// </summary>
public class FeatJusticar : PromotionFeat
{
    public override string PromotionClassId => Constants.JusticarId;
    public override int PromotionClassFeatId => Constants.FeatJusticar;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActIntimidateId,
        Constants.ActSubdueId,
        Constants.ActCondemnId,
        Constants.StJudgementFlameId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActIntimidateId, 60, false);
        c.ability.Add(Constants.ActSubdueId, 60, false);
        c.ability.Add(Constants.ActCondemnId, 60, false);
        c.ability.Add(Constants.StJudgementFlameId, 60, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "inquisitor";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
}