using System.Collections.Generic;
using NierMod.Common;
using UnityEngine;
namespace NierMod.Feats;

internal class FeatMyBeloved : Feat
{
    public override Sprite GetIcon(string suffix = "")
    {
        return SpriteSheet.Get(source.alias);
    }

    public override List<string> Apply(int a, ElementContainer owner, bool hint = false)
    {
        if (!hint && owner.Chara?.id is not Constants.nierCharaId)
        {
            // Removes Feat if not on Nier
            owner.Remove(id);
        }
        return base.Apply(a, owner, hint);
    }
}