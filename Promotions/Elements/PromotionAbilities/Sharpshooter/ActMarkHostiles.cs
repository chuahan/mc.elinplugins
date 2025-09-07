using PromotionMod.Common;
using PromotionMod.Stats.Sharpshooter;
namespace PromotionMod.Elements.PromotionAbilities.Sharpshooter;

public class ActMarkHostiles : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSharpshooter) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SharpshooterId.lang()));
            return false;
        }

        return base.CanPerform();
    }

    public override bool Perform()
    {
        int manaRestore = (int)(CC.mana.max * 0.05F);
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(TP, 3F, CC, false, true))
        {
            target.AddCondition<ConMarked>();
            CC.mana.Mod(manaRestore);
        }
        return true;
    }
}