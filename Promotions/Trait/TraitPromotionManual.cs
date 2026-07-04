using System.Collections.Generic;

using PromotionMod.Common;
namespace PromotionMod.Trait;

public class TraitPromotionManual : TraitScroll
{
    public override bool CanRead(Chara c)
    {
        return true;
    }

    public override void OnRead(Chara c)
    {
        if (!TraitPromotionManual.CanPromote(c)) return;
        List<string> characterPromotes = TraitPromotionManual.GetPromotionOptions(c);
        characterPromotes.Add("promotion_cancel");
        Dialog.List("promotion_choices".lang(c.NameSimple), characterPromotes, j => j, delegate(int idx, string option)
        {
            if (option == "promotion_cancel")
            {
                return true;
            }
            ShowPromotionDetails(option, c);
            return true;
        }, true);
    }

    public void ShowPromotionDetails(string promotionId, Chara c)
    {
        string promotionInformationLookup = promotionId + "_info";
        Dialog.YesNo(promotionInformationLookup.lang() + "\n\r\n\r" + "promotion_confirmation".lang(c.NameSimple), delegate
        {
            if (promotionId == Constants.JeneiId)
            {
                JeneiPromotionAttunement(c);
            }
            else
            {
                Promote(promotionId, c);
            }
        });
    }

    public void JeneiPromotionAttunement(Chara c)
    {
        // Promoting a Farmer into a Jenei requires you to select one of the four elements.
        // These will add two Psynergy abilities, and for NPCs they will also add Arrow/Ball/Sword of that Element.
        // 0 - Venus/Earth/Impact
        // 1 - Mars/Fire
        // 2 - Jupiter/Lightning
        // 3 - Mercury/Water/Cold
        List<string> jeneiAttunements = new List<string>
        {
            "jenei_venus",
            "jenei_mars",
            "jenei_jupiter",
            "jenei_mercury",
            "promotion_cancel"
        };
        Dialog.List("jenei_promotions".lang(), jeneiAttunements, j => j, delegate(int idx, string option)
        {
            if (option == "promotion_cancel")
            {
                return true;
            }

            // Add the Attunement Flag
            c.SetInt(Constants.JeneiAttunementFlag, idx);
            Promote(Constants.JeneiId, c);
            return true;
        }, true);
    }

    public void Promote(string promotionId, Chara c)
    {
        // TODO Text : Add Promote Text.
        c.SetFeat(Constants.PromotionIdMap[promotionId]);
        c.SetInt(Constants.PromotionFeatFlag, Constants.PromotionIdMap[promotionId]);
        // The Promotion Feat should handle the abilities for both PC and NPCs once it gets added with OnApply.
        c.PlaySound("godbless");
        c.PlayEffect("aura_heaven");
        c.Say("spellbookCrumble", owner.Duplicate(1));
        owner.ModNum(-1);
    }

    public static List<string> GetPromotionOptions(Chara c)
    {
        // Some classes don't work really well for NPCs.
        bool isPC = c.IsPC;
        List<string> promotionIds;
        switch (c.job.id)
        {
            case "warrior":
                promotionIds = new List<string>
                {
                    Constants.SentinelId,
                    Constants.BerserkerId
                };
                break;
            case "thief":
                promotionIds = new List<string>
                {
                    Constants.HermitId,
                    Constants.TricksterId
                };
                break;
            case "wizard":
                promotionIds = new List<string>
                {
                    Constants.ElementalistId,
                    Constants.NecromancerId
                };
                break;
            case "farmer":
                promotionIds = new List<string>
                {
                    Constants.JeneiId,
                    Constants.DruidId
                };
                break;
            case "archer":
                promotionIds = new List<string>
                {
                    Constants.SniperId,
                    Constants.RangerId
                };
                break;
            case "warmage":
                promotionIds = new List<string>
                {
                    Constants.BattlemageId,
                    Constants.DreadKnightId
                };
                break;
            case "pianist":
                promotionIds = new List<string>
                {
                    Constants.DancerId,
                    Constants.KnightcallerId
                };
                break;
            case "priest":
                promotionIds = new List<string>
                {
                    Constants.SaintId,
                    Constants.WarClericId
                };
                break;
            case "gunner":
                promotionIds = new List<string>
                {
                    Constants.SharpshooterId,
                    Constants.MachinistId
                };
                break;
            case "inquisitor":
                promotionIds = new List<string>
                {
                    Constants.WitchHunterId,
                    Constants.JusticarId
                };
                break;
            case "paladin":
                promotionIds = new List<string>
                {
                    Constants.SovereignId,
                    Constants.HolyKnightId
                };
                break;
            case "witch":
                promotionIds = new List<string>
                {
                    Constants.HexerId,
                    Constants.ArtificerId
                };
                break;
            case "executioner":
                promotionIds = new List<string>
                {
                    Constants.HeadhunterId,
                    Constants.HarbingerId
                };
                break;
            case "swordsage":
                promotionIds = new List<string>
                {
                    Constants.RuneKnightId,
                    Constants.SpellbladeId
                };
                break;
            case "tourist": // Tourist Can go into any class.
                promotionIds = new List<string>
                {
                    Constants.AdventurerId,
                    Constants.SentinelId,
                    Constants.BerserkerId,
                    Constants.HermitId,
                    Constants.TricksterId,
                    Constants.ElementalistId,
                    Constants.NecromancerId,
                    Constants.JeneiId,
                    Constants.DruidId,
                    Constants.SniperId,
                    Constants.RangerId,
                    Constants.BattlemageId,
                    Constants.RuneKnightId,
                    Constants.DancerId,
                    Constants.KnightcallerId,
                    Constants.SaintId,
                    Constants.WarClericId,
                    Constants.SharpshooterId,
                    Constants.MachinistId,
                    Constants.WitchHunterId,
                    Constants.JusticarId,
                    Constants.SovereignId,
                    Constants.HolyKnightId,
                    Constants.HeadhunterId,
                    Constants.HarbingerId,
                    Constants.SpellbladeId,
                    Constants.DreadKnightId,
                    Constants.HexerId,
                    Constants.ArtificerId
                };
                break;
            default:
                promotionIds = new List<string>(); // Return nothing.
                break;
        }

        // Remove PC only classes
        if (!isPC)
        {
            promotionIds.Remove(Constants.AdventurerId);
            promotionIds.Remove(Constants.ArtificerId);
        }

        // Add Custom Unlock Classes
        if (pc.GetBool(Constants.GamblerPromotionUnlockedFlag)) promotionIds.Add(Constants.GamblerId);

        return promotionIds;
    }

    public static bool CanPromote(Chara c)
    {
        if (c is { IsPC: false, _affinity: < 75 })
        {
            Msg.Say("promotion_affinityrequirement".langGame(c.Name));
            return false;
        }

        if (c.LV < Constants.PromotionLevelRequirement)
        {
            Msg.Say("promotion_levelrequirement".langGame(c.Name, Constants.PromotionLevelRequirement.ToString()));
            return false;
        }

        // If the character is in a class that cannot promote
        if (TraitPromotionManual.GetPromotionOptions(c).Count == 0)
        {
            Msg.Say("promotion_unsupportedclass".langGame(c.Name));
            return false;
        }

        int promotedFeat = c.GetInt(Constants.PromotionFeatFlag, 0);
        if (promotedFeat != 0 && c.Evalue(promotedFeat) > 0)
        {
            Msg.Say("promotion_alreadypromoted".langGame(c.Name));
            return false;
        }

        return true;
    }
}