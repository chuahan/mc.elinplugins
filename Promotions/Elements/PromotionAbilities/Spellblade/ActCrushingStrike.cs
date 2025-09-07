using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Spellblade;

public class ActCrushingStrike : ActMelee
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSpellblade) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SpellbladeId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    // TODO (P3) I kind of want this one to be SP.
    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        // Snapshot the HP Before and After.
        int currentHP = TC.hp;

        // Get the body parts of the target.
        BodySlot partTarget = TC.Chara.body.slots.RandomItem();
        int breakAmount = (int)HelperFunctions.SigmoidScaling(GetPower(CC), 10, 25);
        Attack();

        // If the HP changed after the attack, we'll consider it a hit.
        if (TC.hp < currentHP)
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