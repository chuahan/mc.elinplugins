using UnityEngine;
namespace PromotionMod.Stats.WitchHunter;

public class ConNullZone : BaseBuff
{
    public override bool TimeBased => true;
    public override bool CanManualRemove => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    public override void Tick()
    {
        // Apply Sanctuary to everyone within 3F.
        foreach (Chara chara in pc.currentZone.map.ListCharasInCircle(owner.pos, 3F))
        {
            Condition? nullZone = chara.GetCondition<ConNullPresence>() ?? chara.AddCondition<ConNullPresence>();
            if (nullZone is { value: > 1 })
            {
                continue;
            }

            nullZone?.Mod(1);
        }
        base.Tick();
    }
}