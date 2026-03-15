using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Stats;

public class ConGreaterRegen : Timebuff
{
    public override ConditionType Type => ConditionType.Buff;
    public virtual float MinHealing => 0.1F;
    public virtual float MaxHealing => 0.25f;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public float GetHealingPercentage()
    {
        return HelperFunctions.SigmoidScaling(power, MinHealing, MaxHealing);
    }

    public override void Tick()
    {
        owner.HealHP((int)(owner.MaxHP * GetHealingPercentage()), HealSource.HOT);
        base.Tick();
    }
}