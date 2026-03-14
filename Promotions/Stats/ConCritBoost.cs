using UnityEngine;
namespace PromotionMod.Stats;

public class ConCritBoost : ConBuffStats
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    public override ConditionType Type => ConditionType.Buff;
    public override bool TimeBased => true;
}