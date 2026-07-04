using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Trait.Characters;
namespace PromotionMod.Source.Drama;

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
    [ElinDramaActionInvoke]
    private static bool TeaHouse_CanOrderTea(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        if (EClass.pc.HasCondition<ConTeaTime>())
        {
            return false;
        }

        return true;
    }

    [ElinDramaActionInvoke]
    private static bool TeaHouse_OrderTea(DramaManager dm, Dictionary<string, string> line, string teaName)
    {
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
    [ElinDramaActionInvoke]
    private static bool SmithyUshrirStateCheck(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        var ushrir = dm.GetChara(line["actor"]);
        EClass.pc.SetInt(SmithySpecialTopicsFlag, 0);

        // If Lailah is in the Party and hasn't had the interaction.
        if (EClass.pc.GetBool(SmithyLailahConversationCompleteFlag))
        {
            if (EClass.pc.party.members.Any(x => x.id == Constants.LailahCharaId))
            {
                EClass.pc.SetInt(SmithyLailahConversationReadyFlag, 1);
                EClass.pc.SetInt(SmithySpecialTopicsFlag, 1);
            }   
        }
        return false;
    }

    [ElinDramaActionInvoke]
    private static bool SmithyUshrirRunicHammerEnchant(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        var ushrir = dm.GetChara(line["actor"]);
        if (EClass.pc.GetInt(SmithyRechargeIntroFlag, 0) == 0)
        {
            ushrir.ShowDialog("ushrir", "ushrirHammerFirstUse");            
        }

        return true;
    }
    
    [ElinDramaActionInvoke]
    private static bool SmithyUshrirModUnlockRangedWeapon(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        var ushrir = dm.GetChara(line["actor"]);
        if (EClass.pc.GetInt(SmithyRechargeIntroFlag, 0) == 0)
        {
            ushrir.ShowDialog("ushrir", "ushrirHammerFirstUse");            
        }

        return true;
    }
    #endregion

    #region Dining Hall
    
    [ElinDramaActionInvoke]
    private static bool DiningHallKariStateCheck(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        var grandmaCat = dm.GetChara(line["actor"]);
        if (grandmaCat.GetInt(WarmHearthFreeFoodTimerFlag, 0) < EClass.world.date.GetRaw())
        {
            grandmaCat.SetInt(WarmHearthFreeFoodTimerFlag, 0);
            if (EClass.pc.party.members.Any(c => c.hunger.GetPhase() >= StatsHunger.VeryHungry))
            {
                grandmaCat.SetInt(WarmHearthPartyHungryFlag, 1);
            }
        }

        // If there's a daily special load it.
        int dailySpecial = EClass.world.date.day % 7;
        grandmaCat.SetInt(WarmHearthDailySpecialFlag, dailySpecial is >= 1 and <= 5 ? dailySpecial : 0);
        return false;
    }

    [ElinDramaActionInvoke]
    private static bool DiningHall_EatFreeFood(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Shove a meal into all the hungry party members.
        var grandmaCat = dm.GetChara(line["actor"]);
        foreach (Chara c in EClass.pc.party.members.Where(c => c.hunger.GetPhase() >= StatsHunger.VeryHungry))
        {
            Msg.Say("grandmaCat_foodGift".langGame(c.NameSimple));
            Thing meal = TraitGrandmaCat.MakeGrandmaLunch(grandmaCat);
            c.AddCard(meal);
            c.SetAIImmediate(new AI_Eat
            {
                target = meal
            });
            grandmaCat.SetInt(WarmHearthPartyHungryFlag, 0);
            // Add a cooldown to prevent this from being used, can only be used once per day.
            grandmaCat.SetInt(WarmHearthFreeFoodTimerFlag, EClass.world.date.GetRaw() + 1440);
        }

        return false;
    }
    #endregion
    
    #region Boutique
    [ElinDramaActionInvoke]
    private static bool BoutiqueAlderStateCheck(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        var chara = dm.GetChara(line["actor"]);
        return false;
    }
    
    [ElinDramaActionInvoke]
    private static bool BoutiqueAlderRepairClothing(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        var chara = dm.GetChara(line["actor"]);
        return false;
    }
    
    [ElinDramaActionInvoke]
    private static bool BoutiqueAlderRefitClothing(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        var chara = dm.GetChara(line["actor"]);
        return false;
    }
    #endregion
}