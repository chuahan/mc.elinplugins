using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Spellblade;

public class ActMeleeCrushingStrike : ActMelee
{
    public override bool UseWeaponDist => false;

    public override bool Perform()
    {
        // Get the body parts of the target.
        BodySlot partTarget = TC.Chara.body.slots.RandomItem();
        int breakAmount = (int)HelperFunctions.SigmoidScaling(GetPower(CC), 10, 25);
        bool hasHit = AttackProcess.Current.Perform(1, false);

        if (hasHit)
        {
            int power = GetPower(CC);

            // Depending on the Body Part, attempt to inflict different condition(s).
            CC.Say("spellblade_crushing_strike".lang(CC.NameSimple, TC.NameSimple, partTarget.name));
            switch (partTarget.elementId)
            {
                case 30: // Head
                    TC.Chara.AddCondition<ConBlind>(power);
                    TC.Chara.AddCondition<ConFaint>(power);
                    break;
                case 31: // Neck
                    TC.Chara.AddCondition<ConSilence>(power);
                    break;
                case 32: // Torso
                case 33: // Back
                case 37: // Waist
                    TC.Chara.AddCondition(SubPoweredCondition.Create(nameof(ConArmorBreak), power, breakAmount));
                    break;
                case 34: // Arm
                case 35: // Hand
                case 36: // Finger
                    TC.Chara.AddCondition<ConDisable>(power);
                    break;
                case 38: // Leg
                case 39: // Foot
                    TC.Chara.AddCondition(SubPoweredCondition.Create(nameof(ConSpeedBreak), power, breakAmount));
                    break;
            }
        }

        return true;
    }
}