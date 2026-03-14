using UnityEngine;
namespace PromotionMod.Stats.HolyKnight;

public class StanceVanguard : BaseStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public override void Tick()
    {
        // If the users HP falls below 25%, automatically exit Vanguard Stance.
        if (Owner.hp <= (int)(Owner.MaxHP * 0.25F)) Kill();
    }
}