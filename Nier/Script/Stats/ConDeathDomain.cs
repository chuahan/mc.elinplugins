using UnityEngine;
namespace NierMod.Stats;

internal class ConDeathDomain : BaseBuff
{
    public override string TextDuration => "";
    public override bool CanManualRemove => false;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}