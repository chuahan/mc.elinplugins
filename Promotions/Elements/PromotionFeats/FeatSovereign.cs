using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     A king is no king without his people, but a people without their king would be lost as well. The Sovereign is a
///     frontline leader that is as much a tactician as they are a warrior.
///     Sovereigns focus on observing the battle and commanding their forces as the situation sees fit with their commands
///     and aura.
///     They specialize in the use of Law and Chaos Stances and the subsequent Orders to change the flow of battle.
///     All Orders are on a Cooldown of 10 Turns and will last 10 turns.
///     Skill - Law Stance - When Law Stance is in play. Defensive Orders will be given. In addition, all allies within 5
///     radius will take 5% reduced damage.
///     Will deactivate Chaos Stance.
///     Skill - Chaos Stance - When Chaos Stance is in play. Offensive Orders will be given. In addition, all allies within
///     5 radius will deal 5% increased damage.
///     Will deactivate Law Stance.
///     Skill - Morale Orders - To Victory! / To Death!
///     - Law: Increase to all Attributes, Life and Mana.
///     - Chaos: Speed Increase, Increase Casting/Combat skills.
///     Skill - Strategy Orders - Rally to me / Rout the Enemy!
///     - Law: Burst heals allies, gives them Protection, then slowly regenerates their HP / MP.
///     - Chaos: Allies gain Holy Intonation, Vorpal. When characters under Rout order make a kill they recover 10% of
///     their MP and SP, as well as restore charges of their intonation if possible.
///     Skill - Formation Orders - Barricade Formation / Sword Formation
///     Coherency will link allies to their own minions or other allies.
///     - Law: When allies suffer damage, the damage is reduced by 5% per neighboring friendly that is also in Formation.
///     - Chaos: When allies make a Melee attack or Ranged Attack, one ally in Formation will try to make a ranged or melee
///     attack followup.
///     The follow-up attack can only happen once per turn.
/// </summary>
public class FeatSovereign : PromotionFeat
{
    public override string PromotionClassId => Constants.SovereignId;
    public override int PromotionClassFeatId => Constants.FeatSovereign;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.StLawModeId,
        Constants.StChaosModeId,
        Constants.ActMoraleOrderId,
        Constants.ActStrategyOrderId,
        Constants.ActFormationOrderId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.StLawModeId, 75, false);
        c.ability.Add(Constants.StChaosModeId, 75, false);
        c.ability.Add(Constants.ActMoraleOrderId, 85, false);
        c.ability.Add(Constants.ActStrategyOrderId, 85, false);
        c.ability.Add(Constants.ActFormationOrderId, 85, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "paladin";
    }

    protected override void ApplyInternal()
    {
        // Strategy
        // Heavy Armor
        // Tactics
        // Charisma
        owner.Chara.elements.ModPotential(286, 30);
        owner.Chara.elements.ModPotential(304, 30);
    }
}