using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.HolyKnight;
namespace PromotionMod.Elements.PromotionAbilities.HolyKnight;

public class ActLuminousDeflection : AIAct
{
    public override bool CancelWhenDamaged => false;
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHolyKnight) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HolyKnightId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActLuminousDeflectionId)) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            type = CostType.None,
            cost = 0
        };
    }

    public override bool CanManualCancel()
    {
        return true;
    }

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
                CC.AddCondition<ConLuminousDeflection>();
                CC.AddCooldown(Constants.ActLuminousDeflectionId, 3);
            }
        }.SetDuration(2);
        yield return Do(seq);
    }
}