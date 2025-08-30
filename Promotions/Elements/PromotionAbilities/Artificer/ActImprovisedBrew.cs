using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Artificer;

/// <summary>
/// Artificer Ability
/// Cook up a random potion and throw it at the target.
/// If target is an ally, will be beneficial. 1/10 Chance of being an extremely beneficial brew.
/// If target is an enemy, will be harmful. 1/4 Chance of being a... special brew.
/// </summary>
public class ActImprovisedBrew : Ability
{
    private static List<string> NegativePotions = new List<string>
    {
        "330", // Blindness
        "331", // Confusion
        "334", // Sleeping
        "335", // Paralysis
        "330" // Poison
    };

    private static List<int> SpecialNegativePotions = new List<int>
    {
        8700, // Sleep
        8702, // Weakness,
        8790, // Broomification
        8791, // Putitfication
        8792 // Catification
    };

    private static List<int> PositivePotions = new List<int>
    {
        8403, // Heal
        8502, // Holy Shield
        8504, // Hero
        8510 // Speed
    };

    private static List<int> SpecialPositivePotions = new List<int>
    {
        8406, // Jure Heal
        8550 // Phoenix
    };

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHexer) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HexerId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        if (TC != null && TC.isChara)
        {
            Chara target = TC.Chara;
            if (target.IsHostile(CC))
            {
                Thing negativePotion;
                if (EClass.rnd(4) == 0)
                {
                    negativePotion = ThingGen.Create(NegativePotions.RandomItem());
                }
                else
                {
                    negativePotion = ThingGen.CreatePotion(SpecialNegativePotions.RandomItem());
                }

                ActThrow.Throw(CC, TC.pos, negativePotion, ThrowMethod.Punish);
            }
            else
            {
                Thing positivePotion;
                if (EClass.rnd(50) == 0)
                {
                    positivePotion = ThingGen.CreatePotion(SpecialPositivePotions.RandomItem());
                }
                else
                {
                    positivePotion = ThingGen.CreatePotion(PositivePotions.RandomItem());
                }

                ActThrow.Throw(CC, TC.pos, positivePotion, ThrowMethod.Punish);
            }
        }
        return true;
    }
}