using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

public class StMistDance : Ability
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}