using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Source.Drama;

internal class MinariDramaExpansion : DramaOutcome
{
    private static bool MinariStateCheck(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Set Affinity Flags
        HelperFunctions.SetAffinityFlags(Constants.MinariCharaId);
        return true;
    }
}