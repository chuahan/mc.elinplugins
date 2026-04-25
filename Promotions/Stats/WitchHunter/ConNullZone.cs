using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Stats.WitchHunter;

public class ConNullZone : ConAura
{
    public override bool TimeBased => true;
    public override bool CanManualRemove => true;

    public override bool TimedAura => true;
    public override AuraType AuraTarget => AuraType.Both;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void OnRemoved()
    {
        // Add a cooldown on expiration.
        CC.AddCooldown(Constants.ActNullZoneId, 10);
    }

    public override void ApplyFriendly(Chara target)
    {
        Condition? nullPresence = target.GetCondition<ConNullPresence>() ?? target.AddCondition<ConNullPresence>();
        if (nullPresence is { value: > 1 }) return;
        nullPresence?.Mod(1);
    }

    public override void ApplyFoe(Chara target)
    {
        Condition? nullPresence = target.GetCondition<ConNullPresence>() ?? target.AddCondition<ConNullPresence>();
        if (nullPresence is { value: > 1 }) return;
        nullPresence?.Mod(1);
    }
}