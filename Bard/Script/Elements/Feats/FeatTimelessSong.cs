using System.Collections.Generic;
using BardMod.Common;
using UnityEngine;
namespace BardMod.Elements.Feats;

public class FeatTimelessSong : Feat
{
    public override Sprite GetIcon(string suffix = "")
    {
        return SpriteSheet.Get(source.alias);
    }

    public override List<string> Apply(int a, ElementContainer owner, bool hint = false)
    {
        if (!hint && owner.Chara?.id is not Constants.SelenaCharaId)
        {
            // Removes Feat if not on Selena
            owner.Remove(id);
        }
        return base.Apply(a, owner, hint);
    }
}