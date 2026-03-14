using UnityEngine;
namespace PromotionMod.Stats.WarCleric;

public class ConDivineDescent : BaseBuff
{
    public override bool TimeBased => true;
    
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    
    public override RendererReplacer GetRendererReplacer()
    {
        return RendererReplacer.CreateFrom("angel_mode");
    }
}