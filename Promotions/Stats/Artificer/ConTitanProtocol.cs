using UnityEngine;
namespace PromotionMod.Stats.Artificer;

public class ConTitanProtocol : BaseBuff
{
    public override string TextDuration => "";
    
    public override void Tick()
    {
        if (owner.host == null)
        {
            this.Kill();
        }
    }
    
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}