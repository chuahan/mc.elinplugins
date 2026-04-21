using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Hexer;

/// <summary>
///     Attempts to consume a debuff on the Hexer and does damage equal to it's power against a target.
///     User must have a debuff to remove.
/// </summary>
public class ActRevenge : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatHexer;
    public override string PromotionString => Constants.HexerId;
    public override int AbilityId => Constants.ActRevengeId;
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool CanPerformExtra()
    {
        if (TC is not { isChara: true }) return false;
        return CC.Chara.conditions.Count(con => con.Type is ConditionType.Debuff) != 0;
    }

    public override bool Perform()
    {
        List<Condition> negativeConditions = CC.Chara.conditions.Where(con => con.Type == ConditionType.Debuff).ToList();
        foreach (Condition debuff in negativeConditions)
        {
            if (debuff.Type != ConditionType.Debuff || debuff.IsKilled || EClass.rnd(GetPower(CC) * 2) <= EClass.rnd(debuff.power)) continue;
            CC.Say("removeHex", TC, debuff.Name.ToLower());
            // Damage Target based on the debuff power + this spellpower.
            int combinedPower = debuff.power + GetPower(CC);
            ActEffect.DamageEle(CC, EffectId.Arrow, combinedPower, Element.Create(Constants.EleMind, combinedPower / 10), new List<Point>
            {
                TC.pos
            }, new ActRef
            {
                act = this
            });
            debuff.Kill();
            return true;
        }
        return true;
    }
}