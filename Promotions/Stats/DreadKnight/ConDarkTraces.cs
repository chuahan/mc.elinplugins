namespace PromotionMod.Stats.DreadKnight;

public class ConDarkTraces : ClassCondition
{
    public override int MaxStacks => 10;
    public override int DecayDelayMax => 3;
    public override string TextDuration => GetStacks().ToString();
}