using System.Linq;
using Cwl.API.Attributes;
using Cwl.API.Processors;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
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

        // For V1.02, if Lailah is already spawned, set her Promotion flag so she can use her abilities if necessary.
        Chara? lailahCharacter = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.LailahCharaId);
        if (lailahCharacter != null)
        {
            if (lailahCharacter.GetFlagValue(Constants.PromotionFeatFlag) == 0 && lailahCharacter.HasElement(Constants.FeatSharpshooter))
            {
                lailahCharacter.SetFlagValue(Constants.PromotionFeatFlag, Constants.FeatSharpshooter);
            }
        }
    }
}