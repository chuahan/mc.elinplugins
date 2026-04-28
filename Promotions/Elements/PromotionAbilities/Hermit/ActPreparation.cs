using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Hermit;
namespace PromotionMod.Elements.PromotionAbilities.Hermit;

/// <summary>
///     This ability can only be used on targets that have been Marked for Death.
///     Targeted debuff that also grants the Hermit Crit Boost, while inflicting one of: Sleep, Poison, Paralyze, Bleed, or
///     Faint. The chance of inflicting the debuff increases depending on how high the Stalk value of Mark for Death is.
/// </summary>
public class ActPreparation : PromotionCombatAbility
{

    private static List<string> _possibleDebuffs = new List<string>
    {
        nameof(ConParalyze),
        nameof(ConBleed),
        nameof(ConSleep),
        nameof(ConFaint),
        nameof(ConPoison)

    };

    public override int PromotionId => Constants.FeatHermit;
    public override string PromotionString => Constants.HermitId;
    public override int AbilityId => Constants.ActPreparationId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool CanPerformExtra(bool verbose)
    {
        // Must have a Target. Target must be marked for death with at least 10 value.
        if (TC is not { isChara: true }) return false;

        ConMarkedForDeath deathMark = TC.Chara.GetCondition<ConMarkedForDeath>();
        if (deathMark == null)
        {
            if (CC.IsPC && verbose) Msg.Say("hermit_preparation_mustbestalking".langGame());
            return false;
        }
        return true;
    }

    public override bool Perform()
    {
        ConMarkedForDeath deathMark = TC.Chara.GetCondition<ConMarkedForDeath>();
        if (deathMark == null) return false;

        int calcPower = GetPower(CC);
        int boostPower = (int)HelperFunctions.SigmoidScaling(calcPower, 10, 50);
        CC.AddCondition(SubPoweredCondition.Create(nameof(ConCritBoost), calcPower, boostPower));

        int stalkBonusMultiplier = deathMark.value / 10;

        ActEffect.ProcAt(EffectId.Debuff, calcPower * stalkBonusMultiplier, BlessedState.Normal, CC, TC, TC.pos, true, new ActRef
        {
            origin = CC.Chara,
            n1 = _possibleDebuffs.RandomItem()
        });
        return true;
    }
}