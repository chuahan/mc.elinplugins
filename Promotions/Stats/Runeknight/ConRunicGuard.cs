using UnityEngine;
namespace PromotionMod.Stats.Runeknight;

public class ConRunicGuard : BaseBuff
{
    public override bool TimeBased => true;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    // Does nothing but activate for Elemental Attunement.
    public override void Tick()
    {
    }
}