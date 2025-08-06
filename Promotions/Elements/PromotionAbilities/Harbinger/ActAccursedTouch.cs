using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Harbinger;
namespace PromotionMod.Elements.PromotionAbilities.Harbinger;

/// <summary>
/// Harbinger Ability
/// Applies one of the Harbinger Miasmas to the target.
///  Will not apply a miasma that the target already has.
/// </summary>
public class ActAccursedTouch : Ability
{
    internal static List<string> PossibleMiasmas = new List<string>
    {
        nameof(ConBlindingMiasma),
        nameof(ConChillingMiasma),
        nameof(ConDebilitatingMiasma),
        nameof(ConRendingMiasma),
        nameof(ConSmotheringMiasma)
    };

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHarbinger) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HarbingerId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        if (TC != null && TC.isChara) ActAccursedTouch.AddMiasma(GetPower(CC), CC, TC.Chara);
        return true;
    }

    internal static void AddMiasma(int power, Chara caster, Chara target)
    {
        Random rng = new Random();
        List<string> activeMiasma = target.conditions.Select(t => nameof(t)).ToList();
        List<string> inactiveMiasma = PossibleMiasmas.Except(activeMiasma).ToList();
        if (inactiveMiasma.Count == 0) return;
        Condition miasma = Condition.Create(inactiveMiasma[rng.Next(inactiveMiasma.Count)], power);
        TC.Chara.AddCondition(miasma);
    }
}