using PromotionMod.Stats.Dancer;
using UnityEngine;
namespace PromotionMod.Elements.Abilities.Dancer;

public class StanceHealingDance : StanceDance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void ActInternal(Chara target, int dancePower, bool isPartner)
    {
        // Heals 10% HP a turn.
        if (isPartner)
        {
            owner.HealHP((int)(target.MaxHP * 0.15F), HealSource.HOT);
        }
        else
        {
            owner.HealHP((int)(target.MaxHP * 0.1F), HealSource.HOT);
        }
    }
}