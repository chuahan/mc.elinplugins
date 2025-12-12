namespace PromotionMod.Stats;

/// <summary>
/// Basically it's really hard to do "on hit" attacks.
/// So where I need to do these types of mechanics, the ability adds a "dummy" condition that gets used up later.
/// These are all meant to be consumed by the patches to add additional effects.
/// </summary>
public class ConAttackResolveCondition : BaseBuff 
{
    public override void Tick()
    {
        this.Kill();
    }
}