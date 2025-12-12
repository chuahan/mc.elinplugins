using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Harbinger;
namespace PromotionMod.Elements.PromotionAbilities.Harbinger;

/// <summary>
///     Harbinger Ability
///     Applies one of the Harbinger Miasmas to the target.
///     Will not apply a miasma that the target already has.
/// </summary>
public class ActAccursedTouch : Ability
{
    internal static List<string> PossibleMiasmas = new List<string>
    {
        nameof(ConBlindingMiasma), // Blind
        nameof(ConChillingMiasma), // Chill
        nameof(ConDebilitatingMiasma), // Weakness
        nameof(ConRendingMiasma), // Bleed
        nameof(ConSmotheringMiasma), // Paralyze
        nameof(ConDisorientingMiasma) // Confuse
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
        if (TC is { isChara: true }) ActAccursedTouch.AddMiasma(GetPower(CC), CC, TC.Chara);
        return true;
    }

    internal static void AddMiasma(int power, Chara caster, Chara target)
    {
        List<string> activeMiasma = target.conditions.Select(t => nameof(t)).ToList();
        List<string> inactiveMiasma = PossibleMiasmas.Except(activeMiasma).ToList();
        if (inactiveMiasma.Count == 0)
        {
            target.Say("harbinger_max_affliction".langGame());
            return;
        }
        ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, CC, target, target.pos, true, new ActRef
        {
            origin = CC.Chara,
            n1 = inactiveMiasma.RandomItem()
        });
    }
}