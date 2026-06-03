using System;
using System.Collections.Generic;
using System.Linq;
using NierMod.Common;
using UnityEngine;

namespace NierMod
{
    internal class NierDramaExpansion : DramaOutcome
    {
        static bool nier_change(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
        {
            var nier = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.nierCharaId);
            if (nier is not null)
            {
                if (!parameters.IsEmpty())
                {
                    int flag = 1;
                    if (Int32.TryParse(parameters[0], out flag))
                    {
                        nier.idSkin = flag;
                    }
                }
                else
                {
                    nier.idSkin = ((nier.idSkin != 1) ? 1 : 2);
                }
            }
            return false;
        }
    }
}
