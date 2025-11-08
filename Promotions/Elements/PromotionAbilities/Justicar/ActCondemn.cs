using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Justicar;

/// <summary>
///     Justicar Ability
///     Condemn targets a point and attempts to entangle all enemies around that area, dealing damage.
///     For each target in the area, add Protection to the Justicar and all allies around them.
/// </summary>
public class ActCondemn : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJusticar) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.JusticarId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        // Can I play SFX Chains here?
        int condemnedTargets = 0;
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(TP, 3F, CC, false, true))
        {
            ActEffect.ProcAt(EffectId.Debuff, GetPower(Act.CC), BlessedState.Normal, Act.CC, target, target.pos, isNeg: true, new ActRef
            {
                origin = Act.CC.Chara,
                n1 = nameof(ConEntangle),
            });
            
            // Inflict Bane.
            ActEffect.ProcAt(EffectId.Debuff, GetPower(Act.CC), BlessedState.Normal, Act.CC, target, target.pos, isNeg: true, new ActRef
            {
                origin = Act.CC.Chara,
                n1 = nameof(ConBane),
            });
            
            int damage = HelperFunctions.SafeDice(Constants.CondemnAlias, GetPower(CC));
            target.DamageHP(damage, AttackSource.Melee, CC);
            condemnedTargets++;
        }

        int protectionAmount = condemnedTargets * ConProtection.CalcProtectionAmount(GetPower(CC));
        foreach (Chara ally in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, true))
        {
            ally.AddCondition<ConProtection>(protectionAmount);
        }

        return true;
    }
}