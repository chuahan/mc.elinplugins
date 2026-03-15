using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolSonic : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_sonicbomb";

    public override float EffectRadius => 2;

    public virtual void MarkMapHighlights(bool shouldHighlight, Point target)
    {
        _map.ForeachSphere(target.x, target.z, EffectRadius, delegate(Point p)
        {
            if (!p.HasBlock && shouldHighlight)
            {
                p.SetHighlight(8);
            }
        });
    }

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(pos, EffectRadius, cc, false, false);
        pos.PlayEffect("scream");
        foreach (Chara target in targets)
        {
            if (target.IsHostile(cc))
            {
                ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, Act.CC, target, target.pos, true, new ActRef
                {
                    origin = Act.CC.Chara,
                    n1 = nameof(ConDim)
                });
            }
        }
        return true;
    }
}