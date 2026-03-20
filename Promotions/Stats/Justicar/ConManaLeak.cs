using UnityEngine;

namespace PromotionMod.Stats.Justicar;

public class ConManaLeak : Timebuff
{
    public override ConditionType Type => ConditionType.Debuff;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}