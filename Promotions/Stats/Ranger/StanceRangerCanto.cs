using UnityEngine;
namespace PromotionMod.Stats.Ranger;

public class StanceRangerCanto : BaseStance
{
    public override bool TimeBased => true;
    
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public override void Tick()
    {
        // If the owner is no longer riding, cancel Canto
        if (owner.ride == null)
        {
            //TODO TEXT: Canto stop.
            Kill();
        }
    }
}