namespace PromotionMod.Stats.Artificer;

public class ConAerialVision : BaseBuff
{
    public const int FOVBuffAmount = 2;

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
    }
}