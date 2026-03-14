using System.Collections.Generic;
using UnityEngine;
namespace PromotionMod.Stats.Machinist;

public class StanceHeavyarms : BaseStance
{
    public const int FOVBuffAmount = 2;

    public override bool TimeBased => true;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public override void OnStartOrStack()
    {
        owner.RecalculateFOV();
    }

    public override void OnCalculateFov(Fov fov, ref int radius, ref float power)
    {
        radius += FOVBuffAmount;
    }

    public override void OnRemoved()
    {
        owner.RecalculateFOV();
        owner.RemoveCondition<ConGravity>();
    }

    public override void Tick()
    {
        if (_zone.IsRegion)
        {
            // Not allowed in regions.
            Kill();
        }

        // If there are no enemies within range, deactivate Heavyarms.
        List<Point> targets = new List<Point>();
        foreach (Point p in owner.fov.ListPoints())
        {
            foreach (Chara c in p.Charas)
            {
                if (!c.IsHostile(owner)) continue;
                targets.Add(p);
                break;
            }
        }
        if (targets.Count != 0)
        {
            if (CC.IsPC) Msg.Say("machinist_heavyarms_exiting".langGame());
            Kill();
        }
        else
        {
            // Every turn applies Gravity to the user.
            owner.AddCondition<ConGravity>(force: true);

            // Every turn consumes 5% of max mana.
            owner.mana.Mod((int)(owner.mana.max * -0.05F));
        }

        // Having this condition ticks cooldowns faster.
        owner.TickCooldown();
    }
}