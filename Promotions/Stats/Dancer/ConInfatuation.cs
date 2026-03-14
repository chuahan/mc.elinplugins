using UnityEngine;
namespace PromotionMod.Stats.Dancer;

// Reduce hit chance and cast chance.
public class ConInfatuation : Timebuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public override void Tick()
    {
        if (!Owner.IsPC)
        {
            Owner.ModAffinity(pc, 1, false, true);
        }
        base.Tick();
    }
}