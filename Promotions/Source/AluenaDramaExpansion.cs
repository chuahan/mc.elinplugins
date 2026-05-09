using System;
using System.Collections.Generic;
using System.Linq;
using Cwl.API.Drama;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Trait.Characters;
namespace PromotionMod.Source;

internal class AluenaDramaExpansion : DramaOutcome
{
    private const string SmithySpecialTopicsFlag = "ushSpTopics";
    private const string SmithyHammerReadyFlag = "ushHamRdy";
    private const string SmithyRechargeReadyFlag = "ushRechRdy";
    private const string SmithyRechargeIntroFlag = "ushRechIntro";
    private const string SmithyRechargeUnlockedFlag = "ushRechUnlck";
    private const string SmithyLailahConversationReadyFlag = "ushLaiRdy";
    private const string SmithyLailahConversationCompleteFlag = "ushLaiComplete";
    
    private const string WarmHearthDailySpecialFlag = "whDay";
    private const string WarmHearthFreeFoodTimerFlag = "whTime";
    private const string WarmHearthPartyHungryFlag = "partyHungry";
    
    #region Teahouse 
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
    #endregion
    
    #region Smithy
    private static bool SmithyUshrirStateCheck(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara ushrir);
        EClass.pc.SetFlagValue(SmithySpecialTopicsFlag, 0);

        // If Lailah is in the Party and hasn't had the interaction.
        if (EClass.pc.GetFlagValue(SmithyLailahConversationCompleteFlag) > 0)
        {
            if (EClass.pc.party.members.Any(x => x.id == Constants.LailahCharaId))
            {
                EClass.pc.SetFlagValue(SmithyLailahConversationReadyFlag, 1);
                EClass.pc.SetFlagValue(SmithySpecialTopicsFlag, 1);
            }   
        }
        return false;
    }

    private static bool SmithyUshrirRunicHammerEnchant(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara ushrir);
        if (EClass.pc.GetFlagValue(SmithyRechargeIntroFlag) == 0)
        {
            ushrir.ShowDialog("ushrir", "ushrirHammerFirstUse");            
        }

        return true;
    }
    #endregion

    #region Dining Hall
    private static bool DiningHallKariStateCheck(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out Chara grandmaCat);
        if (grandmaCat.GetFlagValue(WarmHearthFreeFoodTimerFlag) < EClass.world.date.GetRaw())
        {
            grandmaCat.SetFlagValue(WarmHearthFreeFoodTimerFlag, 0);
            if (EClass.pc.party.members.Any(c => c.hunger.GetPhase() >= StatsHunger.VeryHungry))
            {
                grandmaCat.SetFlagValue(WarmHearthPartyHungryFlag, 1);
            }
        }

        // If there's a daily special load it.
        int dailySpecial = EClass.world.date.day % 7;
        grandmaCat.SetFlagValue(WarmHearthDailySpecialFlag, dailySpecial is >= 1 and <= 5 ? dailySpecial : 0);
        return false;
    }

    private static bool DiningHall_EatFreeFood(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Shove a meal into all the hungry party members.
        dm.RequiresActor(out Chara grandmaCat);
        foreach (Chara c in EClass.pc.party.members.Where(c => c.hunger.GetPhase() >= StatsHunger.VeryHungry))
        {
            Msg.Say("grandmaCat_foodGift".langGame(c.NameSimple));
            Thing meal = TraitGrandmaCat.MakeGrandmaLunch(grandmaCat);
            c.AddCard(meal);
            c.SetAIImmediate(new AI_Eat
            {
                target = meal
            });
            grandmaCat.SetFlagValue(WarmHearthPartyHungryFlag, 0);
            // Add a cooldown to prevent this from being used, can only be used once per day.
            grandmaCat.SetFlagValue(WarmHearthFreeFoodTimerFlag, EClass.world.date.GetRaw() + 1440);
        }

        return false;
    }
    #endregion
}