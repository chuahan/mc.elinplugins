using PromotionMod.Stats.Dancer;
using UnityEngine;
namespace PromotionMod.Elements.Abilities.Dancer;

public class StanceMistDance : StanceDance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void ActInternal(Chara target, int dancePower, bool isPartner)
    {
        Condition? danceBuff = target.GetCondition<ConMistDance>() ?? target.AddCondition<ConMistDance>(dancePower);
        if (danceBuff is { value: >= 3 }) return;
        danceBuff?.Mod(1);
    }
}