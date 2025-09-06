using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// A champion of technological revolution. The Machinist uses technology and firearms to face the oncoming threats with overwhelming firepower.
/// Machinists focus on the creative use of firearms with modifications and automation.
/// They specialize in active modifications to their weapons to improve their performance for the situation at hand.
/// Skill - Load Up - Reloads the currently equipped gun with custom ammunition. Reload will also apply to all nearby turret allies.
///     AP Rounds - Vorpal
///     Salamander Rounds - Fire damage conversion, Inflicts Burn
///     Tracer Rounds - Increased Accuracy.
/// Skill - Overlock - Machinist and all Machine-Type allies + summons will gain boost.
/// Spell - Summon Turret - Deploys one of four different gun turrets. They come with a gun. They have Duration.
///     Nightfall : Stationary Rifle Turret.
///     Echo : Stationary Railgun Turret.
///     Liger Tail : Stationary Rocket Launcher
/// Passive - Conspectus of Machinery - Converts Summon Spellbooks into Summon Auto Turret
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