using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Artificer;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolHeaven : TraitArtificerTool
{
    public override string ArtificerToolId => "artificier_heavenpearl";

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(cc.pos, 3F, cc, true, true);
        float healingAmount = HelperFunctions.SigmoidScaling(power, 10F, 40F);
        foreach (Chara target in targets)
        {
            int heal = (int)(target.MaxHP * healingAmount);
            target.HealHP(heal, HealSource.Item);
            target.AddCondition<ConHeavenlyEmbrace>(power); // TODO IMPLEMENT
        }
        owner.c_ammo--;
        return true;
    }
}