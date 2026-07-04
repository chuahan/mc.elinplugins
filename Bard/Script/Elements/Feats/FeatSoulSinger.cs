using System.Collections.Generic;
using BardMod.Common;
using UnityEngine;
namespace BardMod.Elements.Feats;

public class FeatSoulSinger : Feat
{
    public override Sprite GetIcon(string suffix = "")
    {
        return SpriteSheet.Get(source.alias);
    }

    public override List<string> Apply(int a, ElementContainer owner, bool hint = false)
    {
        if (!hint && owner.Chara?.id is not Constants.NiyonCharaId)
        {
            // Removes Feat if not on Niyon
            owner.Remove(id);
        }
        return base.Apply(a, owner, hint);
    }
}