using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Maia;
namespace PromotionMod.Elements.Maia;

/// <summary>
/// Inflicts slow on all enemies within a radius. Adds Magic Resistance to the caster.
/// </summary>
public class ActEnlightenedSilentForce : Ability
{
    public override bool CanPerform()
    {
        // Ability is only usable by ascended Maia.
        if (CC.Evalue(Constants.FeatMaia) == 0 || CC.Evalue(Constants.FeatMaiaEnlightened) == 0)
        {
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        // We assume at this point since we got past CanPerform that the Maia is ascended.
        int power = this.GetPower(CC);
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 3F, CC, false, true);
        foreach (Chara target in targets)
        {
            if (target.IsHostile())
            {
                ActEffect.ProcAt(EffectId.DebuffStats, power, BlessedState.Normal, CC, target, target.pos, true, new ActRef()
                {
                    act = this,
                    n1 = "SPD",
                });
            }
        }
        
        CC.AddCondition(SubPoweredCondition.Create(nameof(ConMagicResBoost), power, 10));
        return true;
    }
}