using Cwl.Helper.Extensions;
using UnityEngine;
namespace PromotionMod.Stats.WarCleric;

public class ConDivineDescent : BaseBuff
{
    public override bool TimeBased => true;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void OnStart()
    {
        base.OnStart();
        if (owner != null)
        {
            // Praise religion
        }
        if (!owner.IsPCC)
        {
            owner.SetSpriteOverride("angel_mode");
        }
        owner.PlaySound("boost2");
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        owner.SetSpriteOverride(null);
    }
}