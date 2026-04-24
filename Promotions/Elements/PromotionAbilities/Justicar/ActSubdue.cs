using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Justicar;
namespace PromotionMod.Elements.PromotionAbilities.Justicar;

/// <summary>
///     Justicar Ability
///     The Justicar inflicts Attack Break, Suppress, and Excommunication on the target.
///     If you are good aligned, you will inflict the Mana Leak Debuff that will cause attacks to steal MP from the target.
///     If you are evil aligned, you will purge one active buff on the target.
/// </summary>
public class ActSubdue : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatJusticar;
    public override string PromotionString => Constants.JusticarId;
    public override int AbilityId => Constants.ActSubdueId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;
    
    public override bool CanPerformExtra()
    {
        return ACT.Melee.CanPerform();
    }

    public override bool Perform()
    {
        // Get Karma Scores for the Player.
        // NPCs will be considered 0 Karma.
        bool positiveKarma = false, negativeKarma = false;
        if (CC.IsPCFactionOrMinion || CC.IsPC)
        {
            // For PC Faction, use PC's Karma.
            positiveKarma = player.karma >= 0;
            negativeKarma = player.karma < 0;
        }

        int calcPower = GetPower(CC);
        int breakAmount = (int)HelperFunctions.SigmoidScaling(calcPower, 10, 25);
        TC.Chara.AddCondition(SubPoweredCondition.Create(nameof(ConAttackBreak), calcPower, breakAmount));

        // Inflict Bane
        ActEffect.ProcAt(EffectId.Debuff, calcPower, BlessedState.Normal, CC, TC, TC.pos, true, new ActRef
        {
            origin = CC.Chara,
            n1 = nameof(ConBane)
        });

        // Inflict Suppress
        ActEffect.ProcAt(EffectId.Debuff, calcPower, BlessedState.Normal, CC, TC, TC.pos, true, new ActRef
        {
            origin = CC.Chara,
            n1 = nameof(ConSupress)
        });

        if (positiveKarma)
        {
            ActEffect.ProcAt(EffectId.Debuff, calcPower, BlessedState.Normal, CC, TC, TC.pos, true, new ActRef
            {
                origin = CC.Chara,
                n1 = nameof(ConManaLeak)
            });
        }

        if (negativeKarma && TC.isChara)
        {
            foreach (Condition condition in TC.Chara.conditions.Copy())
            {
                if (condition.Type == ConditionType.Buff &&
                    !condition.IsKilled &&
                    EClass.rnd(calcPower * 2) > EClass.rnd(condition.power))
                {
                    CC.Say("purgeBuff".langGame(), TC.Chara, condition.Name.ToLower());
                    condition.Kill();
                    break;
                }
            }
        }
        return true;
    }
}