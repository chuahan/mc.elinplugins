using UnityEngine;
namespace PromotionMod.Stats.Headhunter;

public class ConHeadhunter : ClassCondition
{
    public override int MaxStacks => 10;
    public override bool TimeBased => true;
    public override int DecayDelayMax => 5;

    public override string TextDuration => this.GetStacks().ToString();
}