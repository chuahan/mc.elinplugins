using UnityEngine;
namespace BardMod.Elements.Feats;

public class FeatBard : Feat
{
    public override Sprite GetIcon(string suffix = "")
    {
        return SpriteSheet.Get(source.alias);
    }
}