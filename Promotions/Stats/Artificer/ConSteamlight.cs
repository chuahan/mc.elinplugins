using UnityEngine;
namespace PromotionMod.Stats.Artificer;

public class ConSteamlight : BaseBuff
{
    public override void OnRemoved()
    {
        CC.AddCondition<ConBurnout>();
        base.OnRemoved();
    }

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}