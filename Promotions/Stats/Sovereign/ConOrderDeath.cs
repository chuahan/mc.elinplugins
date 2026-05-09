using UnityEngine;
namespace PromotionMod.Stats.Sovereign;

public class ConOrderDeath : BaseBuff
{
    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public override void OnStart()
    {
        owner.RemoveCondition<ConOrderVictory>();
        base.OnStart();
    }
}