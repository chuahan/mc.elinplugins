using System.Collections.Generic;
using Newtonsoft.Json;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Stats;

/// <summary>
///     Aura Conditions will constantly apply the effect to nearby characters every turn.
/// </summary>
public abstract class ConAura : BaseBuff
{

    public enum AuraType
    {
        Friendly,
        Foe,
        Both
    }

    [JsonProperty(PropertyName = "T")] public int TriggerTick;

    public override bool TimeBased => true;

    public virtual float AuraRadius => 3F;

    public virtual AuraType AuraTarget => AuraType.Friendly;

    public virtual int IdAbility => -1;

    public virtual bool TimedAura => false;

    public virtual int TriggerDelay => 0;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

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

        // If there's an ability attached, level up the related skill.
        Element element = owner.elements.GetElement(IdAbility);
        if (element != null)
        {
            owner.elements.ModExp(element.id, 20f);
            power = element.GetPower(owner); // Refresh the live power from the ability
        }

        (List<Chara> friendlies, List<Chara> enemies) = HelperFunctions.GetOrganizedCharasWithinRadius(owner.pos, AuraRadius, owner, true);
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

        if (TimedAura)
        {
            base.Tick();
        }
    }
}