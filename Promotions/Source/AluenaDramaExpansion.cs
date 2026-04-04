using System;
using System.Collections.Generic;
using System.Linq;
using Cwl.API.Drama;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Source;

internal class AluenaDramaExpansion : DramaOutcome
{
    private static bool TeaHouse_CanOrderTea(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        if (EClass.pc.HasCondition<ConTeaTime>())
        {
            return false;
        }

        return true;
    }
    
    private static bool TeaHouse_OrderTea(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        parameters.Requires(out string teaName);
        
        // Deduct Orens
        if (!EClass.pc.TryPay(100 * EClass.pc.party.Count()))
        {
            return false;
        } 
        
        Enum.TryParse(teaName, out ConTeaTime.TeaType teaType);
        
        // Apply the Tea Condition to everyone.
        foreach (Chara partyMember in EClass.pc.party.members)
        {
            partyMember.AddCondition(new ConTeaTime
            {
                TeaFlavor = teaType
            });
            partyMember.AddCondition<ConAwakening>(500);
        }

        return true;
    }

    private static bool DiningHall_FreeFoodEligible(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        if (EClass.pc.party.members.Any(c => c.hunger.GetPhase() >= StatsHunger.Hungry))
        {
            return true;
        }
        
        return false;
    }

    private static bool DiningHall_EatFreeFood(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Feed all the hungry party members.
        foreach (Chara c in EClass.pc.party.members.Where(c => c.hunger.GetPhase() >= StatsHunger.Hungry))
        {
            
        }
        return false;
    }
}