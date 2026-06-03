using NierMod.Common;
using UnityEngine;

namespace NierMod.Feats
{
    internal class FeatLoversDeathsDemise : Feat
    {
        public override Sprite GetIcon(string suffix = "")
        {
            return SpriteSheet.Get(source.alias);
        }

        internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
        {
            if (owner.Chara?.id is not Constants.nierCharaId)
            {
                // Removes Feat if not on Nier
                owner.Remove(id);
                return;
            }
        }
    }
}