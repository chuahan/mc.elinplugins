using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Stats.Sovereign;

public class StanceChaosSovereign : StanceSovereign
{
    public override void Tick()
    {
        // Every turn apply Sovereign Chaos buff.
        foreach (Condition con in HelperFunctions.GetCharasWithinRadius(owner.pos, 5F, owner, true, true)
                         .Select(target => target.GetCondition<ConSovereignChaos>() ?? target.AddCondition<ConSovereignChaos>()).Where(con => con is not { value: >= 1 }))
        {
            con?.Mod(1);
        }
        base.Tick();
    }
}