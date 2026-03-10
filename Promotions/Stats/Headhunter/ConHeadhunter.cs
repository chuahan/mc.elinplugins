using Newtonsoft.Json;
namespace PromotionMod.Stats.Headhunter;

public class ConHeadhunter : ClassCondition
{
    public override int MaxStacks => 10;

    public override bool TimeBased => true;
    
    private const int DecayDelayMax = 5;

    [JsonProperty(PropertyName = "R")] private int _decayDelay;
    
    public override void Tick()
    {
        if (_decayDelay == DecayDelayMax)
        {
            // Decay stacks
            TickStacks();
        }
        else if (_decayDelay < DecayDelayMax)
        {
            _decayDelay++;
        }
    }
}