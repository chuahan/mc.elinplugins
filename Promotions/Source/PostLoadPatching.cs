using System.Linq;
using Cwl.API.Attributes;
using Cwl.API.Processors;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Source;

internal class PostLoadPatching : EClass
{
    [CwlPostLoad]
    internal static void PromotionMod_Reapply(GameIOProcessor.GameIOContext context)
    {
        if (!core.IsGameStarted)
        {
            return;
        }

        if (pc != null)
        {
            // In the situation where the user uninstalled the mod due for some reason, then continued, the promotion feats themselves would be removed.
            // However, since the promotion FLAG is set, we can actually use that to restore it.
            foreach (Chara chara in game.cards.globalCharas.Values.Where(gc => gc.GetFlagValue(Constants.PromotionFeatFlag) != 0))
            {
                int promotionId = chara.GetFlagValue(Constants.PromotionFeatFlag);
                if (!chara.HasElement(promotionId))
                {
                    Msg.Say("promotionmod_patching".langGame());
                    chara.SetFeat(promotionId);
                }
            }

            // For testing purposes, activate Gambler for me cause Evie isn't fully implemented.
            // EClass.pc.SetFlagValue(Constants.GamblerPromotionUnlockedFlag, 1);
        }
    }

    [CwlPostSave]
    internal static void PromotionMod_PatchSyncing(GameIOProcessor.GameIOContext context)
    {
        if (!core.IsGameStarted)
        {
            return;
        }

        if (pc != null)
        {
            int version = pc.GetFlagValue(Constants.PromotionModVersionFlag);
            if (version < 108)
            {
                // If Lailah is already spawned, set her Promotion flag so she can use her abilities if necessary.
                Chara lailah = pc.currentZone.FindChara(Constants.LailahCharaId);
                if (lailah != null)
                {
                    if (lailah.GetFlagValue(Constants.PromotionFeatFlag) == 0 && lailah.HasElement(Constants.FeatSharpshooter))
                    {
                        Msg.Say("promotionmod_patching".langGame());
                        lailah.SetFlagValue(Constants.PromotionFeatFlag, Constants.FeatSharpshooter);
                    }
                }
                //Update all the Knightcaller Captains with their respective promotion feat flags.
                PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.DinatogCharaId, Constants.FeatSniper);
                PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.AlestieCharaId, Constants.FeatHermit);
                PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.ValeroCharaId, Constants.FeatSentinel);
                PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.EctoleCharaId, Constants.FeatSaint);
                PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.ArkunCharaId, Constants.FeatSpellblade);
                PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.DiasCharaId, Constants.FeatHeadhunter);
                PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.RolingerCharaId, Constants.FeatBattlemage);
                
                // Snipers, Hermits, and Alraunes get new abilities. Plus we need to clean up the duplicates.
                PostLoadPatching.PromotionMod_PatchHermitSniper();
                PostLoadPatching.PromotionMod_PatchAlraune();
                
                // Update the Flag Value so we can skip this stuff next time.
                pc.SetFlagValue(Constants.PromotionModVersionFlag, Constants.PromotionModVersion);
            }
        }
    }

    internal static void PromotionMod_PatchCharaPromo(string charaId, int promotionFeatId)
    {
        Chara characterToPatch = pc.currentZone.FindChara(charaId);
        if (characterToPatch != null)
        {
            if (characterToPatch.GetFlagValue(Constants.PromotionFeatFlag) == 0 && characterToPatch.HasElement(promotionFeatId))
            {
                Msg.Say("promotionmod_patching".langGame());
                characterToPatch.SetFlagValue(Constants.PromotionFeatFlag, promotionFeatId);
            }
        }
    }

        internal static void PromotionMod_PatchHermitSniper()
    {
        foreach (Chara chara in game.cards.globalCharas.Values
                         .Where(gc => gc.GetFlagValue(Constants.PromotionFeatFlag) == Constants.FeatHermit ||
                                      gc.GetFlagValue(Constants.PromotionFeatFlag) == Constants.FeatSniper))
        {
            if (chara.GetFlagValue(Constants.PromotionFeatFlag) == Constants.FeatHermit)
            {
                if (chara.IsPC)
                {
                    if (!chara.HasElement(Constants.ActPreparationId))
                    {
                        Msg.Say("promotionmod_patching".langGame());
                        chara.AddElement(Constants.ActPreparationId);
                    }
                }

                else
                {
                    if (!chara.ability.Has(Constants.ActPreparationId))
                    {
                        Msg.Say("promotionmod_patching".langGame());
                        chara.ability.Add(Constants.ActPreparationId, 75, false);
                    }
                    else if (chara.ability.Has(Constants.ActPreparationId))
                    {
                        chara._listAbility = chara._listAbility.Distinct().ToList();
                        chara.ability.Refresh();
                    }
                }
            }

            if (chara.GetFlagValue(Constants.PromotionFeatFlag) == Constants.FeatSniper)
            {
                if (chara.IsPC)
                {
                    if (!chara.HasElement(Constants.ActTacticalRetreatId))
                    {
                        Msg.Say("promotionmod_patching".langGame());
                        chara.AddElement(Constants.ActTacticalRetreatId);
                    }
                }

                else
                {
                    if (!chara.ability.Has(Constants.ActTacticalRetreatId))
                    {
                        Msg.Say("promotionmod_patching".langGame());
                        chara.ability.Add(Constants.ActTacticalRetreatId, 75, false);
                    }
                    else if (chara.ability.Has(Constants.ActTacticalRetreatId))
                    {
                        chara._listAbility = chara._listAbility.Distinct().ToList();
                        chara.ability.Refresh();
                    }
                }
            }
        }
    }

    internal static void PromotionMod_PatchAlraune()
    {
        foreach (Chara chara in game.cards.globalCharas.Values.Where(gc => gc.HasElement(Constants.FeatAlraune)))
        {
            if (chara.IsPC)
            {
                if (!chara.HasElement(Constants.ActAlrauneConsumeId))
                {
                    Msg.Say("promotionmod_patching".langGame());
                    chara.AddElement(Constants.ActAlrauneConsumeId);
                }
            }
            else
            {
                if (!chara.ability.Has(Constants.ActAlrauneConsumeId))
                {
                    Msg.Say("promotionmod_patching".langGame());
                    chara.ability.Add(Constants.ActAlrauneConsumeId, 75, false);
                }
                else if (chara.ability.Has(Constants.ActAlrauneConsumeId))
                {
                    chara._listAbility = chara._listAbility.Distinct().ToList();
                    chara.ability.Refresh();
                }
            }
        }
    }
}