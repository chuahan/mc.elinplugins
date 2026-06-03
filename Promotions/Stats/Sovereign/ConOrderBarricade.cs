using UnityEngine;
namespace PromotionMod.Stats;

public class ConOrderBarricade : BaseBuff
{

    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void OnStart()
    {
        owner.RemoveCondition<ConOrderSword>();
        base.OnStart();
    }
}