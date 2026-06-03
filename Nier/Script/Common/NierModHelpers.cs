using System;

namespace NierMod.Common
{
    public static class NierModHelpers
    {
        public static bool IsNierAndMarried(Chara character)
        {
            return (character.id == Constants.nierCharaId) && 
                   EClass.player.dialogFlags.TryGetValue("nierMarried", 0) >= 1;
        }
    }
}

