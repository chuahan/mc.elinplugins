using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Sharpshooter;
namespace PromotionMod.Stats.Sovereign;

public class StanceLawSovereign : StanceSovereign
{
    public override void Tick()
    {
        // Every turn apply Sovereign Law buff.
        foreach (Condition con in HelperFunctions.GetCharasWithinRadius(owner.pos, 5F, owner, true, true)
                         .Select(target => target.GetCondition<ConSovereignLaw>() ?? target.AddCondition<ConSovereignLaw>()).Where(con => con is not { value: >= 1 }))
        {
            con?.Mod(1);
        }
        base.Tick();
    }
}