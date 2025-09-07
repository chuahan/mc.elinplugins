using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Harbinger;
namespace PromotionMod.Elements.PromotionAbilities.Harbinger;

/// <summary>
///     Harbinger Ability
///     Extends the duration of all the Miasma debuffs on nearby enemies.
/// </summary>
public class ActEndlessMists : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHarbinger) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HarbingerId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActEndlessMistsId)) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true))
        {
            List<Condition> conditions = target.conditions.Where(x => x is ConHarbingerMiasma or ConMiasma).ToList();
            foreach (Condition condition in conditions)
            {
                condition.Mod(1);
            }
        }

        CC.AddCooldown(Constants.ActEndlessMistsId, 5);
        return true;
    }
}