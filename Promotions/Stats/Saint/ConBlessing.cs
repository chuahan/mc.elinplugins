using UnityEngine;
namespace PromotionMod.Stats.Saint;

public class ConBlessing : BaseBuff
{
    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void OnStart()
    {
        base.OnStart();
        owner.RefreshFaithElement();
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        owner.RefreshFaithElement();
    }
}