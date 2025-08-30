using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Stats.Sentinel;

public class ConTaunted : BaseDebuff
{
    public override ConditionType Type => ConditionType.Debuff;

    public override void Tick()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(owner.pos, 4f, owner, false, true);
        foreach (Chara potentialTarget in targets)
        {
            if (potentialTarget.HasCondition<ConTaunting>())
            {
                owner.SetEnemy(potentialTarget);
                break;
            }
        }
        base.Tick();
    }
}