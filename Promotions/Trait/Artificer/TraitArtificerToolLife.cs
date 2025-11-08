using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolLife : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_lifeflower";

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(cc.pos, 3F, cc, true, true);
        float healingAmount = HelperFunctions.SigmoidScaling(power, 15F, 25F);
        int afterImageCount = (int)HelperFunctions.SigmoidScaling(power, 3F, 6F);
        foreach (Chara target in targets)
        {
            int heal = (int)(target.MaxHP * healingAmount);
            target.HealHP(heal, HealSource.Item);
            target.AddCondition<ConAfterimage>(afterImageCount);
        }
        owner.c_ammo--;
        return true;
    }
}