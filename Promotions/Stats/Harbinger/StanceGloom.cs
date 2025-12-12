using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities.Harbinger;
namespace PromotionMod.Stats.Harbinger;

public class StanceGloom : BaseStance
{
    public override bool TimeBased => true;

    public override void Tick()
    {
        if (_zone.IsRegion)
        {
            return;
        }

        List<Chara> affectedEnemies = HelperFunctions.GetCharasWithinRadius(CC.pos, 3, CC, false, false);
        foreach (Chara target in affectedEnemies)
        {
            ActAccursedTouch.AddMiasma(power, owner, target);
        }
    }
}