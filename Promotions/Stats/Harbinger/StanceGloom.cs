using PromotionMod.Elements.PromotionAbilities.Harbinger;
namespace PromotionMod.Stats.Harbinger;

public class StanceGloom : ConAura
{
    public override bool TimeBased => true;

    public override AuraType AuraTarget => AuraType.Foe;
    
    public override void ApplyFoe(Chara target)
    {
        ActAccursedTouch.AddMiasma(power, owner, target);
    }
}