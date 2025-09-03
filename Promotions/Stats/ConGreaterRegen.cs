using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConGreaterRegen : Timebuff
{
    public virtual float MinHealing => 0.1F;
    public virtual float MaxHealing => 0.25f;
    
    public float GetHealingPercentage()
    {
        return HelperFunctions.SigmoidScaling(this.power, MinHealing, MaxHealing);
    }
    
    public override void Tick()
    {
        owner.HealHP((int)(owner.MaxHP * this.GetHealingPercentage()), HealSource.HOT);
        base.Tick();
    }
}