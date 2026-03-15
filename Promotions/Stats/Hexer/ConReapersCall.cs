using UnityEngine;
namespace PromotionMod.Stats.Hexer;

// Timed Debuff Lose 10% HP a turn for 10 turns.
public class ConReapersCall : Timebuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void Tick()
    {
        owner.DamageHP((long)(owner.MaxHP * 0.1F), AttackSource.Condition);
        base.Tick();
    }
}