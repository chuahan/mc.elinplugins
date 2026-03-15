using UnityEngine;
namespace PromotionMod.Stats.WarCleric;

public class ConSanctuary : BaseBuff
{
    public override bool TimeBased => true;
    public override bool CanManualRemove => false;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}