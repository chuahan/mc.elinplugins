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

    public enum AuraType
    {
        Friendly,
        Foe,
        Both,
    }

    public override bool TimeBased => true;

    public virtual int AuraRadius => 3;
    public virtual AuraType AuraTarget => AuraType.Friendly;

    public virtual bool TimedAura => false;

    public virtual int TriggerDelay => 0;

    public virtual void ApplyFriendly(Chara target)
    {

    }
    
    public virtual void ApplyFoe(Chara target)
    {
        
    }

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
        (List<Chara> friendlies, List<Chara> enemies) = HelperFunctions.GetOrganizedCharasWithinRadius(CC.pos, AuraRadius, CC, true);
        switch (AuraTarget)
        {
            case AuraType.Friendly:
                foreach (Chara target in friendlies)
                {
                    ApplyFriendly(target);
                }
                return;
            case AuraType.Foe:
                foreach (Chara target in enemies)
                {
                    ApplyFoe(target);
                }
                return;
            case AuraType.Both:
                foreach (Chara target in friendlies)
                {
                    ApplyFriendly(target);
                }
                foreach (Chara target in enemies)
                {
                    ApplyFoe(target);
                }
                break;
        }
    }
}