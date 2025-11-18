using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Artificer;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolTime : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_timehourglass";

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        (List<Chara> friendlies, List<Chara> enemies) = HelperFunctions.GetOrganizedCharasWithinRadius(cc.pos, 3F, cc, true);
        foreach (Chara target in enemies)
        {
            ActEffect.ProcAt(EffectId.DebuffStats, power, BlessedState.Normal, Act.CC, target, target.pos, true, new ActRef
            {
                origin = Act.CC.Chara,
                n1 = "SPD"
            });
        }

        foreach (Chara target in friendlies)
        {
            ActEffect.ProcAt(EffectId.BuffStats, power, BlessedState.Normal, Act.CC, target, target.pos, false, new ActRef
            {
                origin = Act.CC.Chara,
                n1 = "SPD"
            });

            target.AddCondition<ConAcceleration>();
        }
        owner.c_ammo--;
        return true;
    }
}