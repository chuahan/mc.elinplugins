namespace PromotionMod.Stats.Sniper;

public class ConSniperTarget : BaseBuff
{
    public enum TargetPart
    {
        Hand,
        Head,
        Legs,
    }

    public TargetPart Target;
}