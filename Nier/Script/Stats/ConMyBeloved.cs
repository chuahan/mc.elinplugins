using UnityEngine;
namespace NierMod.Stats;

internal class ConMyBeloved : BaseBuff
{
    public override string TextDuration => "";
    public override bool CanManualRemove => false;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}