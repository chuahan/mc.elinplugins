using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     A champion of technological revolution. The Machinist uses technology and firearms to face the oncoming threats
///     with overwhelming firepower.
///     Machinists focus on the creative use of firearms with modifications and automation.
///     They specialize in active modifications to their weapons to improve their performance for the situation at hand.
///     Skill - Load Up - Reloads the currently equipped gun with custom ammunition. Reload will also apply to all nearby
///     turret allies.
///     1) AP Rounds - Vorpal
///     Alt: Drill Rounds. Inflicts PV Break and Bleed.
///     2) Salamander Rounds - Fire damage conversion, Inflicts Burn
///     Alt: Explosive Rounds. Invokes Fireball on destination, inflicts burn.
///     3) Tracer Rounds - Increased Accuracy.
///     Alt: Inflicts DV Break.
///     Skill - Overlock - Machinist and all Machine-Type allies + summons will gain boost.
///     Skill - Entrenchment Stance - Machinist sets in, activating the Heavyarms support frame.
///     Can only be used if there are hostiles in their FOV.
///     In this stance, they gain 25% speed increase, 25% damage reduction, and cooldowns will tick at 4x the rate, and
///     rapidArrow 2.
///     Reloads will be Instant during this mode.
///     When making ranged attacks (non sub), this will automatically fire a follow-up missile at the target's location.
///     Machinists are automatically afflicted with Gravity while in this state.
///     Machinists lose the ability to move (even if mounted.)
///     Machinists will lose 5% of their mana every turn they remain in this mode.
///     Spell - Summon Turret - Deploys one of four different gun turrets. They come with a gun. They have Duration.
///     Passive - Conspectus of Machinery - Converts Summon Spellbooks into Summon Auto Turret
/// </summary>
public class FeatMachinist : PromotionFeat
{
    public override string PromotionClassId => Constants.MachinistId;
    public override int PromotionClassFeatId => Constants.FeatMachinist;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActLoadUpId,
        Constants.ActOverclockId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActLoadUpId, 75, false);
        c.ability.Add(Constants.ActOverclockId, 50, false);
        c.ability.Add(Constants.SpSummonTurretId, 50, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "gunner";
    }

    protected override void ApplyInternal()
    {
        // Gunning
        owner.Chara.elements.ModPotential(286, 30);
        // Blacksmithing
        // Marksmanship
    }
}