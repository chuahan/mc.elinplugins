using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

public class StFreedomDance : Ability
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}