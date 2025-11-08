using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Artificer;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolTime : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_timehourglass";

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(cc.pos, 3F, cc, true, true);
        foreach (Chara target in targets)
        {
            ActEffect.ProcAt(EffectId.DebuffStats, power, BlessedState.Normal, Act.CC, target, target.pos, true, new ActRef()
            {
                origin = Act.CC.Chara,
                n1 = "SPD",
            });
        }
        owner.c_ammo--;
        return true;
    }
}