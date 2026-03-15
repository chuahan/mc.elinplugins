using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Stats.Sniper;

public class ConNoDistractions : BaseBuff
{

    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void Tick()
    {
        if (HelperFunctions.GetCharasWithinRadius(owner.pos, 3, owner, false, true).Count > 0)
        {
            Kill();
        }
    }
}