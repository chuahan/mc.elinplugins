using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Gambler;

/// <summary>
/// </summary>
public class ActDiceStrike : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatGambler;
    public override string PromotionString => Constants.GamblerId;
    public override int AbilityId => Constants.ActDiceStrikeId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool CanPerformExtra()
    {
        if (CC == TC || TC is not { isChara: true } || CC.Dist(TC) > 1)
        {
            return false;
        }

        return true;
    }

    public override bool Perform()
    {
        // Roll two dice.
        Dice gamble = new Dice
        {
            num = 2,
            sides = 6,
            card = CC
        };

        int result = gamble.Roll();
        switch (result)
        {
            case 2:
                // Heal the target instead.
                CC.Say("gambler_dice_dud".langGame(CC.NameSimple));
                TC.HealHP((int)(TC.MaxHP * 0.25F));
                break;
            case 12:
                CC.Say("gambler_dice_maxroll".langGame(CC.NameSimple));
                new ActMeleeDiceStrike
                {
                    DamageMultiOverride = 2F,
                    ShouldCrit = true
                }.Perform(CC, TC);
                break;
            default:
                CC.Say("gambler_dice_roll".langGame(CC.NameSimple, result.ToString()));
                new ActMeleeDiceStrike
                {
                    DamageMultiOverride = 1 + (result - 7) * 0.1F,
                    ShouldCrit = false
                }.Perform(CC, TC);
                break;
        }

        return true;
    }
}