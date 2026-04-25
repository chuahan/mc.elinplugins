using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Gambler;

public class ActCardThrow : PromotionCombatAbility
{


    private float _effectRadius = 5F;
    public override int PromotionId => Constants.FeatGambler;
    public override string PromotionString => Constants.GamblerId;
    public override int AbilityId => Constants.ActCardThrowId;
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;
    public override bool ShowMapHighlight => true;

    public override bool Perform()
    {
        Thing throwingCard = ThingGen.Create("throwing_card", -1, GetPower(CC));

        Dice gamble = new Dice
        {
            num = 1,
            sides = 100,
            card = CC
        };

        int handValue = 0;
        int aces = 0;
        int handSize = 0;

        while (handValue < 16 && handSize < 5)
        {
            int roll = gamble.Roll() + 1;
            handSize++;

            switch (roll)
            {
                case <= 7: // Ace
                    handValue += 11;
                    aces++;
                    break;
                case <= 37: // 10/Face Card
                    handValue += 10;
                    break;
                case <= 97: // Number Card
                {
                    int index = roll - 38;
                    handValue += 2 + index / 8;
                    break;
                }
                case <= 99: // Strong Card
                    handValue += 10;
                    break;
                default: // 100
                {
                    // Perfect card:
                    // First card → Ace
                    // Second+ → Ten-value
                    if (handSize == 1)
                    {
                        handValue += 11;
                        aces++;
                    }
                    else
                    {
                        handValue += 10;
                    }
                    break;
                }
            }

            // Adjust soft aces if busting
            while (handValue > 21 && aces > 0)
            {
                handValue -= 10;
                aces--;
            }

            // Bust
            if (handValue > 21)
            {
                // Bust
                // Throws a single card made out of paper with no other effects.
                CC.Say("gambler_card_bust".langGame(CC.NameSimple, handValue.ToString()));
                throwingCard.ChangeMaterial("paper");
                CC.ranged = throwingCard;
                ActThrow.Throw(CC, TC.pos, TC, throwingCard);
                return true;
            }
        }

        // Adjust the weight of the throwing card based off of the hand value.
        throwingCard.ChangeWeight(handValue * 20);
        CC.ranged = throwingCard;

        // 5 Card Charlie
        if (handSize == 5)
        {
            // Add 5 to the hand value before multiplying.
            CC.Say("gambler_card_fivecardcharlie".langGame(CC.NameSimple));
            throwingCard.ChangeWeight((handValue + 5) * 20);
            throwingCard.ChangeMaterial("gold");
            ActThrow.Throw(CC, TC.pos, TC, throwingCard);
        }

        // Blackjack (only with 2 cards)
        if (handSize == 2 && handValue == 21)
        {
            // Blackjack
            // Creates 2 ether cards, one with convertImpact and the other with one randomly out of convertFire/Cold/Lightning
            // Throw them at the enemy.
            CC.Say("gambler_card_blackjack".langGame(CC.NameSimple));
            throwingCard.ChangeMaterial("ether");
            throwingCard.ModNum(1, false);

            Thing throwingCard2 = throwingCard.Split(1);
            List<int> convertElements = new List<int>
            {
                ENC.convertFire,
                ENC.convertCold,
                ENC.convertLightning,
                ENC.convertHoly,
                ENC.convertImpact
            };
            throwingCard.elements.ModBase(convertElements.RandomItem(), 50);
            ActThrow.Throw(CC, TC.pos, TC, throwingCard);

            throwingCard2.elements.ModBase(convertElements.RandomItem(), 50);
            CC.ranged = throwingCard2;
            ActThrow.Throw(CC, TC.pos, TC, throwingCard2);
            return true;
        }

        // Stand 17–21
        CC.Say("gambler_card_normal".langGame(CC.NameSimple, handValue.ToString()));
        throwingCard.ChangeMaterial("steel");
        ActThrow.Throw(CC, TC.pos, TC, throwingCard);
        return true;
    }
}