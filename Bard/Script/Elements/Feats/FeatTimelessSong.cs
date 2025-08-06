using BardMod.Common;
using UnityEngine;
namespace BardMod.Elements.Feats;

public class FeatTimelessSong : Feat
{
    public override Sprite GetIcon(string suffix = "")
    {
        return SpriteSheet.Get(source.alias);
    }

    internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        if (owner.Chara?.id is not Constants.SelenaCharaId)
        {
            // Removes Feat if not on Selena
            owner.Remove(id);
        }
    }
}