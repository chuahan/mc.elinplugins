using UnityEngine;
namespace PromotionMod.Stats.WarCleric;

public class ConSanctuary : BaseBuff
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    public override bool TimeBased => true;
    public override bool CanManualRemove => false;
}