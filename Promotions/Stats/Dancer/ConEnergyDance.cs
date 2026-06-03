using UnityEngine;
namespace PromotionMod.Stats;

public class ConEnergyDance : Timebuff
{
    public override bool CanManualRemove => true;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}