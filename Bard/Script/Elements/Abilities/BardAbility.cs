using BardMod.Common;
using BardMod.Common.HelperFunctions;
using UnityEngine;
namespace BardMod.Elements.Abilities;

public class BardAbility : Ability
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override Cost GetCost(Chara c)
    {
        Act.Cost result2 = default(Act.Cost);
        result2.type = Act.CostType.MP;
        
        int num = EClass.curve(Value, 50, 10);
        result2.cost = source.cost[0] * (100 + ((!source.tag.Contains("noCostInc")) ? (num * 3) : 0)) / 100;
        
        // Higher Music skill will reduce mana costs.
        if (c != null)
        {
            int musicSkill = c.Chara.Evalue(Constants.MusicSkill);
            result2.cost *= (100 / (100 + musicSkill));
        }

        if ((c == null || !c.IsPC) && result2.cost > 2)
        {
            result2.cost /= 2;
        }
        
        return result2;
    }
    
    public override int GetPower(Card bard)
    {
        // Get Base Power.
        int basePower = base.GetPower(bard);

        if (bard == null) return basePower;

        float powerMultiplier = 1f;
        
        // Sweet Voice Mutation
        if (bard.Evalue(1522) > 0) powerMultiplier += 0.125f;
        // Husky Voice Mutation
        if (bard.Evalue(1523) > 0) powerMultiplier -= 0.125f;
		
        // Duet Multiplier
        if (bard.IsPCParty || bard.IsPC)
        {
            foreach (Chara partyMember in pc.party.members)
            {
                if (partyMember != CC && partyMember.Evalue(Constants.FeatDuetPartner) > 0)
                {
                    powerMultiplier += 0.5F;
                }
            }
        }
        
        // Bard Multiplier
        if (bard.Evalue(Constants.FeatBardId) > 0) powerMultiplier += 0.25f;
		
        // Apply Multipliers
        basePower = HelperFunctions.SafeMultiplier(basePower,powerMultiplier);
        
        return basePower;
    }
}