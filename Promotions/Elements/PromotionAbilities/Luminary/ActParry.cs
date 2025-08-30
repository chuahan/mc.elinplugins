using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Luminary;
namespace PromotionMod.Elements.PromotionAbilities.Luminary;

public class ActParry : AIAct
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatLuminary) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.LuminaryId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActParryId)) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            type = CostType.None,
            cost = 0,
        };
    }
    
    public override bool CanManualCancel()
    {
        return true;
    }
    
    public override bool CancelWhenDamaged => false;

    public override IEnumerable<Status> Run()
    {
        if (owner == null)
        {
            yield return Cancel();
        }
        Progress_Custom seq = new Progress_Custom
        {
            cancelWhenMoved = false,
            showProgress = true,
            onProgressBegin = delegate
            {
                CC.AddCondition<ConParry>();
                CC.AddCooldown(Constants.ActParryId, 3);
            }
        }.SetDuration(2);
        yield return Do(seq);
    }
}