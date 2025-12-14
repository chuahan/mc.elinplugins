using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.WarCleric;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.HolyKnight;

public class ActBlessedArmaments : ActMelee
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHolyKnight) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HolyKnightId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActBlessedArmamentsId)) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        Condition solBlade = CC.AddCondition<ConSolBlade>();
        return true;
    }
}