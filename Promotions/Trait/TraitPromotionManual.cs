using System.Collections.Generic;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Trait;

public class TraitPromotionManual : TraitScroll
{
    public static Dictionary<string, int> PromotionIdMap = new Dictionary<string, int>
    {
        {
            Constants.SentinelId, Constants.FeatSentinel
        },
        {
            Constants.BerserkerId, Constants.FeatBerserker
        },
        {
            Constants.HermitId, Constants.FeatHermit
        },
        {
            Constants.TricksterId, Constants.FeatTrickster
        },
        {
            Constants.ElementalistId, Constants.FeatElementalist
        },
        {
            Constants.NecromancerId, Constants.FeatNecromancer
        },
        {
            Constants.JeneiId, Constants.FeatJenei
        },
        {
            Constants.DruidId, Constants.FeatDruid
        },
        {
            Constants.SniperId, Constants.FeatSniper
        },
        {
            Constants.RangerId, Constants.FeatRanger
        },
        {
            Constants.BattlemageId, Constants.FeatBattlemage
        },
        {
            Constants.RuneknightId, Constants.FeatRuneknight
        },
        {
            Constants.AdventurerId, Constants.FeatAdventurer
        },
        {
            Constants.DancerId, Constants.FeatDancer
        },
        {
            Constants.KnightcallerId, Constants.FeatKnightcaller
        },
        {
            Constants.SaintId, Constants.FeatSaint
        },
        {
            Constants.WarClericId, Constants.FeatWarCleric
        },
        {
            Constants.SharpshooterId, Constants.FeatSharpshooter
        },
        {
            Constants.MachinistId, Constants.FeatMachinist
        },
        {
            Constants.WitchHunterId, Constants.FeatWitchHunter
        },
        {
            Constants.JusticarId, Constants.FeatJusticar
        },
        {
            Constants.SovereignId, Constants.FeatSovereign
        },
        {
            Constants.LuminaryId, Constants.FeatLuminary
        },
        {
            Constants.HeadhunterId, Constants.FeatHeadhunter
        },
        {
            Constants.HarbingerId, Constants.FeatHarbinger
        },
        {
            Constants.SpellbladeId, Constants.FeatSpellblade
        },
        {
            Constants.PhantomId, Constants.FeatPhantom
        },
        {
            Constants.HexerId, Constants.FeatHexer
        },
        {
            Constants.ArtificerId, Constants.FeatArtificer
        }
    };

    public override bool CanRead(Chara c)
    {
        if (c.isBlind) return false;
        return TraitPromotionManual.CanPromote(c);
    }

    public override void OnRead(Chara c)
    {
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
            Promote(promotionId, c);
        });
    }

    public void Promote(string promotionId, Chara c)
    {
        c.SetFeat(PromotionIdMap[promotionId]);
        c.SetFlagValue(Constants.PromotionFeatFlag, PromotionIdMap[promotionId]);
        // Some abilities are PC only.
        if (c.IsPC)
        {
            // Get appropriate extra skills to add on.
        }
        // Get appropriate extra skills to add on.
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
        switch (c.job.name)
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
                    Constants.SpellbladeId
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
                    Constants.LuminaryId
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
                    Constants.RuneknightId,
                    Constants.PhantomId
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
                    Constants.RuneknightId,
                    Constants.DancerId,
                    Constants.KnightcallerId,
                    Constants.SaintId,
                    Constants.WarClericId,
                    Constants.SharpshooterId,
                    Constants.MachinistId,
                    Constants.WitchHunterId,
                    Constants.JusticarId,
                    Constants.SovereignId,
                    Constants.LuminaryId,
                    Constants.HeadhunterId,
                    Constants.HarbingerId,
                    Constants.SpellbladeId,
                    Constants.PhantomId,
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

        int promotedFeat = c.GetFlagValue(Constants.PromotionFeatFlag);
        if (promotedFeat != 0 && c.Evalue(promotedFeat) > 0)
        {
            Msg.Say("promotion_alreadypromoted".langGame(c.Name));
            return false;
        }

        return true;
    }
}