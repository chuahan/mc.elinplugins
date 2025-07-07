using UnityEngine;
namespace BardMod.Elements.Feats;

public class FeatDuetPartner : Feat
{
    public override Sprite GetIcon(string suffix = "")
    {
        return SpriteSheet.Get(source.alias);
    }
}