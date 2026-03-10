namespace PromotionMod.Stats.WarCleric;

public class ConDivineDescent : BaseBuff
{
    public override bool TimeBased => true;
    
    public override RendererReplacer GetRendererReplacer()
    {
        return RendererReplacer.CreateFrom("angel_mode");
    }
}