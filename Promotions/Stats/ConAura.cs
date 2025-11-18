using System.Collections.Generic;
using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats;

/// <summary>
///     Aura Conditions will constantly apply the effect to nearby characters every turn.
/// </summary>
public abstract class ConAura : BaseBuff
{

    [JsonProperty(PropertyName = "T")] public int TriggerTick;

    public override bool TimeBased => true;

    public virtual int AuraRadius => 3;
    public virtual bool FriendlyAura => true;

    public virtual bool TimedAura => false;

    public virtual int TriggerDelay => 0;

    public abstract void ApplyInternal(Chara target);

    public override void Tick()
    {
        if (_zone.IsRegion)
        {
            return;
        }

        // Some Auras will trigger only every X turns.
        if (TriggerTick < TriggerDelay)
        {
            TriggerTick++;
            return;
        }

        // Reset to 0 and trigger effects.
        TriggerTick = 0;
        List<Chara> affectedCharas = HelperFunctions.GetCharasWithinRadius(CC.pos, AuraRadius, CC, FriendlyAura, true);
        foreach (Chara target in affectedCharas)
        {
            ApplyInternal(target);
        }
    }
}