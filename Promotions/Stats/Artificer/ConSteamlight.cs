namespace PromotionMod.Stats.Artificer;

public class ConSteamlight : BaseBuff
{
    public override void OnRemoved()
    {
        CC.AddCondition<ConBurnout>();
        base.OnRemoved();
    }
}