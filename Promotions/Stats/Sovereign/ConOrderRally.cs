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
        int lifeHeal = (int)(CC.MaxHP * .05F);
        int manaHeal = (int)(CC.mana.max * .05F);
        CC.HealHP(lifeHeal, HealSource.HOT);
        CC.mana.Mod(manaHeal);
        base.Tick();
    }
}