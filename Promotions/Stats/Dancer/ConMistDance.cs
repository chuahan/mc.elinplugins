using UnityEngine;
namespace PromotionMod.Stats.Dancer;

public class ConMistDance : Timebuff
{
    public override bool CanManualRemove => true;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}