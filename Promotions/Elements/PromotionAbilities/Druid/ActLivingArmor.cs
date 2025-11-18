using PromotionMod.Common;
using PromotionMod.Stats.Druid;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Druid;

/// <summary>
///     Druid Ability
/// </summary>
public class ActLivingArmor : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatDruid) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.DruidId.lang()));
            return false;
        }
        if (TC.Chara.IsHostile(CC)) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    // Apply Spell Enhance to this ability.
    public override int GetPower(Card c)
    {
        int power = base.GetPower(c);
        return power * Mathf.Max(100 + c.Evalue(411) - c.Evalue(93), 1) / 100;
    }

    public override bool Perform()
    {
        TC.Chara.AddCondition<ConLivingArmor>(GetPower(CC));
        return true;
    }
}