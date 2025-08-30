using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Luminary;

public class StVanguardStance : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatLuminary) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.LuminaryId.lang()));
            return false;
        }
        return base.CanPerform();
    }
    
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}