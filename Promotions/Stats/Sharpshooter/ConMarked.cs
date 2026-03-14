using UnityEngine;
namespace PromotionMod.Stats.Sharpshooter;

public class ConMarked : BaseDebuff
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
}