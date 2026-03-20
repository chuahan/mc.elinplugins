using System.Linq;
using Cwl.API.Attributes;
using Cwl.API.Processors;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;

namespace PromotionMod.Source;

internal class PostLoadPatching : EClass
{
    [CwlPostLoad]
    internal static void PromotionMod_PatchSyncing(GameIOProcessor.GameIOContext context)
    {
        if (!core.IsGameStarted)
        {
            return;
        }

        if (EClass.pc != null)
        {
            int version = EClass.pc.GetFlagValue(Constants.PromotionModVersionFlag);

            if (version < Constants.PromotionModVersion)
            {
                // For V1.02, if Lailah is already spawned, set her Promotion flag so she can use her abilities if necessary.
                Chara? lailahCharacter = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.LailahCharaId);
                if (lailahCharacter != null)
                {
                    if (lailahCharacter.GetFlagValue(Constants.PromotionFeatFlag) == 0 && lailahCharacter.HasElement(Constants.FeatSharpshooter))
                    {
                        lailahCharacter.SetFlagValue(Constants.PromotionFeatFlag, Constants.FeatSharpshooter);
                    }
                }
        
                // For V1.04, Update all the Knightcaller Captains with their respective promotion feat flags.
                PromotionMod_PatchCharaPromo(Constants.DinatogCharaId, Constants.FeatSniper);
                PromotionMod_PatchCharaPromo(Constants.AlestieCharaId, Constants.FeatHermit);
                PromotionMod_PatchCharaPromo(Constants.ValeroCharaId, Constants.FeatSentinel);
                PromotionMod_PatchCharaPromo(Constants.EctoleCharaId, Constants.FeatSaint);
                PromotionMod_PatchCharaPromo(Constants.ArkunCharaId, Constants.FeatSpellblade);
                PromotionMod_PatchCharaPromo(Constants.DiasCharaId, Constants.FeatHeadhunter);
                PromotionMod_PatchCharaPromo(Constants.RolingerCharaId, Constants.FeatBattlemage);
        
                // For V1.05 - Update Hermits and Snipers with their new abilities.
                PromotionMod_PatchHermitSniper();
            }
            
            // Update the Flag Value so we can skip this stuff.
            EClass.pc.SetFlagValue(Constants.PromotionModVersionFlag, Constants.PromotionModVersion);
        }
    }

    internal static void PromotionMod_PatchCharaPromo(string charaId, int promotionFeatId)
    {
        Chara? characterToPatch = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == charaId);
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