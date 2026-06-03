using Cwl.Helper;
using PromotionMod.Common;
using PromotionMod.Patches;
using UnityEngine;
namespace PromotionMod.Stats;

// Timed Debuff Lose 10% HP a turn for 10 turns.
public class ConReapersCall : Timebuff
{
    public override ConditionType Type => ConditionType.Debuff;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void Tick()
    {
        HelperFunctions.DamageHpWrapper(owner, (long)(owner.MaxHP * 0.1F), 0, 100, AttackSource.Condition, null);
        base.Tick();
    }
}