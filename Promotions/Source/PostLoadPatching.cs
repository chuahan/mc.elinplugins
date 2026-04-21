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
                    chara.SetFeat(promotionId);
                }
            }
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
            // Run this save function when in the same zone to update them...
            // For V1.02, if Lailah is already spawned, set her Promotion flag so she can use her abilities if necessary.
            Chara lailah = pc.currentZone.FindChara(Constants.LailahCharaId);
            if (lailah != null)
            {
                if (lailah.GetFlagValue(Constants.PromotionFeatFlag) == 0 && lailah.HasElement(Constants.FeatSharpshooter))
                {
                    lailah.SetFlagValue(Constants.PromotionFeatFlag, Constants.FeatSharpshooter);
                }
            }

            // For V1.04, Update all the Knightcaller Captains with their respective promotion feat flags.
            PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.DinatogCharaId, Constants.FeatSniper);
            PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.AlestieCharaId, Constants.FeatHermit);
            PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.ValeroCharaId, Constants.FeatSentinel);
            PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.EctoleCharaId, Constants.FeatSaint);
            PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.ArkunCharaId, Constants.FeatSpellblade);
            PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.DiasCharaId, Constants.FeatHeadhunter);
            PostLoadPatching.PromotionMod_PatchCharaPromo(Constants.RolingerCharaId, Constants.FeatBattlemage);

            int version = pc.GetFlagValue(Constants.PromotionModVersionFlag);

            if (version < Constants.PromotionModVersion)
            {
                // For V1.05 - Update Hermits and Snipers with their new abilities.
                PostLoadPatching.PromotionMod_PatchHermitSniper();
            }

            // Update the Flag Value so we can skip this stuff next time.
            pc.SetFlagValue(Constants.PromotionModVersionFlag, Constants.PromotionModVersion);
        }
    }

    internal static void PromotionMod_PatchCharaPromo(string charaId, int promotionFeatId)
    {
        Chara characterToPatch = pc.currentZone.FindChara(charaId);
        if (characterToPatch != null)
        {
            if (characterToPatch.GetFlagValue(Constants.PromotionFeatFlag) == 0 && characterToPatch.HasElement(promotionFeatId))
            {
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
                        chara.AddElement(Constants.ActPreparationId);
                    }
                }

                else
                {
                    chara.ability.Add(Constants.ActPreparationId, 75, false);
                }
            }

            if (chara.GetFlagValue(Constants.PromotionFeatFlag) == Constants.FeatSniper)
            {
                if (chara.IsPC)
                {
                    if (!chara.HasElement(Constants.ActTacticalRetreatId))
                    {
                        chara.AddElement(Constants.ActTacticalRetreatId);
                    }
                }

                else
                {
                    chara.ability.Add(Constants.ActTacticalRetreatId, 50, false);
                }
            }
        }
    }
}