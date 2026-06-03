using UnityEngine;
namespace PromotionMod.Stats;

public class ConOrderVictory : BaseBuff
{
    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public override void OnStart()
    {
        owner.RemoveCondition<ConOrderDeath>();
        base.OnStart();
    }
}