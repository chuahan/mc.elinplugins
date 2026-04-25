using UnityEngine;
namespace PromotionMod.Stats;

public class ConDebilitated : BaseDebuff
{
    public override ConditionType Type => ConditionType.Debuff;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}