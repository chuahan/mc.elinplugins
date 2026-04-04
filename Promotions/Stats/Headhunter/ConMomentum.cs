using UnityEngine;
namespace PromotionMod.Stats.Headhunter;

public class ConMomentum : BaseBuff
{
    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}