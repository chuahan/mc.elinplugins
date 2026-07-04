using System.Collections.Generic;
using System.Linq;
using NierMod.Common;
namespace NierMod;

internal class NierDramaExpansion : DramaOutcome
{
    [ElinDramaActionInvoke]
    private static bool nier_change(DramaManager dm, Dictionary<string, string> line, int flag)
    {
        Chara? nier = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.nierCharaId);
        if (nier is not null)
        {
            nier.idSkin = flag;
        }
        return false;
    }
}