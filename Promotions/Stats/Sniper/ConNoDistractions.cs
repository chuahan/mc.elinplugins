using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Stats.Sniper;

public class ConNoDistractions : BaseBuff
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    
    public override bool TimeBased => true;

    public override void Tick()
    {
        if (HelperFunctions.GetCharasWithinRadius(owner.pos, 3, owner, false, true).Count > 0)
        {
            Kill();
        }
    }
}