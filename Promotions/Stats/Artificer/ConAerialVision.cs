using UnityEngine;
namespace PromotionMod.Stats.Artificer;

public class ConAerialVision : BaseBuff
{
    public const int FOVBuffAmount = 2;

    public override string TextDuration => "";
    
    public override void OnStartOrStack()
    {
        owner.RecalculateFOV();
    }

    public override void OnCalculateFov(Fov fov, ref int radius, ref float power)
    {
        radius += FOVBuffAmount;
    }

    public override void OnRemoved()
    {
        owner.RecalculateFOV();
    }
    
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}