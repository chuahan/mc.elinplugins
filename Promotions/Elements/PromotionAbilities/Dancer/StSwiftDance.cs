using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

public class StSwiftDance
        : Ability
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}