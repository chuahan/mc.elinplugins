using System.Collections.Generic;
namespace PromotionMod.Stats.Sharpshooter;

public class StanceHeavyarms : BaseStance
{
    public const int FOVBuffAmount = 15;

    public override bool TimeBased => true;

    public override void OnStartOrStack()
    {
        owner.RecalculateFOV();
    }

    public override void OnCalculateFov(Fov fov, ref int radius, ref float power)
    {
        if (radius < FOVBuffAmount)
        {
            radius = FOVBuffAmount;
        }
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
            Kill();
        }
        else
        {
            // Every turn applies Gravity to the user.
            owner.AddCondition<ConGravity>(force: true);

            // Every turn consumes 5% of max mana.
            owner.mana.Mod((int)(owner.mana.max * -0.05F));
        }
    }
}