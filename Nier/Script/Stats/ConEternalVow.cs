using UnityEngine;
namespace NierMod.Stats;

internal class ConEternalVow : BaseBuff
{
    public override bool CanManualRemove => false;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}