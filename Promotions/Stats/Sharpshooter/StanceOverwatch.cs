using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Stats.Sharpshooter;

public class StanceOverwatch : BaseStance
{
    public const int FOVBuffAmount = 20;

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
    }

    public override void Tick()
    {
        if (_zone.IsRegion)
        {
            return;
        }

        if (owner.IsAliveInCurrentZone && value > 1)
        {
            foreach (Condition underFire in HelperFunctions.GetCharasWithinRadius(owner.pos, 6F, owner, false, true)
                             .Select(target => target.GetCondition<ConUnderFire>() ?? target.AddCondition<ConUnderFire>()).Where(underFire => underFire is not { value: >= 1 }))
            {
                underFire?.Mod(1);
            }
        }
    }
}