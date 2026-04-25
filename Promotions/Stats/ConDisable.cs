using UnityEngine;
namespace PromotionMod.Stats;

public class ConDisable : BaseDebuff
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}