using UnityEngine;
namespace PromotionMod.Stats.Sovereign;

public class ConOrderRally : BaseBuff
{
    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void Tick()
    {
        int lifeHeal = (int)(owner.MaxHP * .05F);
        int manaHeal = (int)(owner.mana.max * .05F);
        owner.HealHP(lifeHeal, HealSource.HOT);
        owner.mana.Mod(manaHeal);
        base.Tick();
    }
    
    public override void OnStart()
    {
        owner.RemoveCondition<ConOrderRout>();
        base.OnStart();
    }
}