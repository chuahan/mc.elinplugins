using System.Collections.Generic;
using UnityEngine;
namespace PromotionMod.Stats;

public class StanceHeavyarms : PromotionStance
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
            return;
        }

        // If there are no enemies within range, deactivate Heavyarms.
        List<Point> targets = new List<Point>();
        foreach (Point p in owner.fov.ListPoints())
        {
            foreach (Chara c in p.Charas)
            {
                if (!c.isChara || !c.IsHostile(owner)) continue;
                targets.Add(p);
                break;
            }
        }
        
        if (targets.Count == 0)
        {
            if (CC.IsPC) Msg.Say("machinist_heavyarms_exiting".langGame());
            Kill();
            return;
        }
        else
        {
            // Every turn applies Gravity to the user.
            CC.AddCondition<ConGravity>(force: true);

            // Every turn consumes 5% of max mana.
            int manaCost = (int)(owner.mana.max * -0.05F);
            CC.mana.Mod(manaCost);
        }

        // Having this condition ticks cooldowns faster.
        if (CC._cooldowns is { Count: > 0 }) CC.TickCooldown();
    }
}