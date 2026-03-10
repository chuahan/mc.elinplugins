using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Artificer;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolHeaven : TraitArtificerTool
{
    public override string ArtificerToolId => "artificier_heavenpearl";
    
    public override int MaxCharges => 10;

    public override bool IsTargetCast => false;
    public override TargetType TargetType => TargetType.Self;

    public override float EffectRadius => 3;

    public virtual void MarkMapHighlights(bool shouldHighlight, Point target)
    {
        EClass._map.ForeachSphere(target.x, target.z, EffectRadius, delegate(Point p)
        {
            if (!p.HasBlock && shouldHighlight)
            {
                p.SetHighlight(8);
            }
        });
    }
    
    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(cc.pos, EffectRadius, cc, true, true);
        float healingAmount = HelperFunctions.SigmoidScaling(power, 10F, 40F);
        int instanceCount = (int)HelperFunctions.SigmoidScaling(power, 5f, 30f);
        cc.PlaySound("holyveil");
        foreach (Chara target in targets)
        {
            int heal = (int)(target.MaxHP * healingAmount);
            target.HealHP(heal, HealSource.Item);
            target.AddCondition<ConHeavenlyEmbrace>(instanceCount);
        }
        return true;
    }
}