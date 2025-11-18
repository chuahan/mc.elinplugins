using System.Collections.Generic;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolSonic : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_sonicbomb";

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        List<Chara> targets = pos.Charas;
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
        owner.c_ammo--;
        return true;
    }
}