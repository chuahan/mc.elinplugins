using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

public class StEnergyDance : Ability
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}