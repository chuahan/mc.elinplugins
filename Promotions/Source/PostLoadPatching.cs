using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Source;

internal class PostLoadPatching : EClass
{
    [ElinPostLoad]
    internal static void PromotionMod_Reapply(GameIOContext context)
    {
        if (!core.IsGameStarted)
        {
            return;
        }

        if (pc != null)
        {
            // In the situation where the user uninstalled the mod due for some reason, then continued, the promotion feats themselves would be removed.
            // However, since the promotion FLAG is set, we can actually use that to restore it.
            foreach (Chara chara in game.cards.globalCharas.Values.Where(gc => gc.GetInt(Constants.PromotionFeatFlag, 0) != 0))
            {
                int promotionId = chara.GetInt(Constants.PromotionFeatFlag, 0);
                if (!chara.HasElement(promotionId))
                {
                    Msg.Say("promotionmod_patching".langGame());
                    chara.SetFeat(promotionId);
                }
            }

            // For testing purposes, activate Gambler for me cause Evie isn't fully implemented.
            EClass.pc.SetBool(Constants.GamblerPromotionUnlockedFlag, true);
        }
    }

    [ElinPostSave]
    internal static void PromotionMod_PatchSyncing(GameIOContext context)
    {
        if (!core.IsGameStarted)
        {
            return;
        }

        if (pc != null)
        {
            int version = pc.GetInt(Constants.PromotionModVersionFlag);
            if (version < 108)
            {
                // If Lailah is already spawned, set her Promotion flag so she can use her abilities if necessary.
                Chara lailah = pc.currentZone.FindChara(Constants.LailahCharaId);
                if (lailah != null)
                {
                    if (lailah.GetInt(Constants.PromotionFeatFlag, 0) == 0 && lailah.HasElement(Constants.FeatSharpshooter))
                    {
                        Msg.Say("promotionmod_patching".langGame());
                        lailah.SetInt(Constants.PromotionFeatFlag, Constants.FeatSharpshooter);
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
                pc.SetInt(Constants.PromotionModVersionFlag, Constants.PromotionModVersion);
            }
        }
    }

    internal static void PromotionMod_PatchCharaPromo(string charaId, int promotionFeatId)
    {
        Chara characterToPatch = pc.currentZone.FindChara(charaId);
        if (characterToPatch != null)
        {
            if (characterToPatch.GetInt(Constants.PromotionFeatFlag, 0) == 0 && characterToPatch.HasElement(promotionFeatId))
            {
                Msg.Say("promotionmod_patching".langGame());
                characterToPatch.SetInt(Constants.PromotionFeatFlag, promotionFeatId);
            }
        }
    }

        internal static void PromotionMod_PatchHermitSniper()
    {
        foreach (Chara chara in game.cards.globalCharas.Values
                         .Where(gc => gc.GetInt(Constants.PromotionFeatFlag, 0) == Constants.FeatHermit ||
                                      gc.GetInt(Constants.PromotionFeatFlag, 0) == Constants.FeatSniper))
        {
            if (chara.GetInt(Constants.PromotionFeatFlag, 0) == Constants.FeatHermit)
            {
                if (chara.IsPC)
                {
                    if (!chara.HasElement(Constants.ActPreparationId))
                    {
                        Msg.Say("promotionmod_patching".langGame());
                        chara.elements.SetBase(Constants.ActPreparationId, 1);
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

            if (chara.GetInt(Constants.PromotionFeatFlag, 0) == Constants.FeatSniper)
            {
                if (chara.IsPC)
                {
                    if (!chara.HasElement(Constants.ActTacticalRetreatId))
                    {
                        Msg.Say("promotionmod_patching".langGame());
                        chara.elements.SetBase(Constants.ActTacticalRetreatId, 1);
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
                    chara.elements.SetBase(Constants.ActAlrauneConsumeId, 1);
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