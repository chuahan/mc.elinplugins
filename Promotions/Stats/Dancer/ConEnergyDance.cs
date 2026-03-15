using UnityEngine;
namespace PromotionMod.Stats.Dancer;

public class ConEnergyDance : Timebuff
{
    public override bool CanManualRemove => true;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}