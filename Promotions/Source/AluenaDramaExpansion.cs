using System;
using System.Collections.Generic;
using System.Linq;
using Cwl.API.Drama;
using Cwl.Helper.Extensions;
using PromotionMod.Stats;
using PromotionMod.Trait.Characters;
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

    private static bool KariStateCheck(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        if (EClass.pc.party.members.Any(c => c.hunger.GetPhase() >= StatsHunger.Hungry))
        {
            EClass.pc.SetFlagValue("partyHungry");
        }

        return false;
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
        // Shove a meal into all the hungry party members.
        dm.RequiresActor(out Chara grandmaCat);
        foreach (Chara c in EClass.pc.party.members.Where(c => c.hunger.GetPhase() >= StatsHunger.Hungry))
        {
            Msg.Say("grandmaCat_foodGift".langGame(c.NameSimple));
            Thing meal = TraitGrandmaCat.MakeGrandmaLunch(grandmaCat);
            c.AddCard(meal);
            c.SetAIImmediate(new AI_Eat
            {
                target = meal
            });
            EClass.pc.SetFlagValue("partyHungry", 0);
            // Add a cooldown of 1 day.

        }

        return false;
    }
}