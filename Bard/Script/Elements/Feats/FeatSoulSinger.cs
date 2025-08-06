using BardMod.Common;
using UnityEngine;
namespace BardMod.Elements.Feats;

public class FeatSoulSinger : Feat
{
    public override Sprite GetIcon(string suffix = "")
    {
        return SpriteSheet.Get(source.alias);
    }

    internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        if (owner.Chara?.id is not Constants.NiyonCharaId)
        {
            // Removes Feat if not on Niyon
            // TODO: THIS IS SUBJECT TO CHANGE.
            owner.Remove(id);
        }
    }
}