using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Hexer;

// Does Mind Damage equivalent to how many negative conditions are on the enemy. 10 turn cooldown. 20% Mana cost.
public class ActTraumatize : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHexer) == 0) return false;
        if (CC.hp <= (CC.MaxHP * 0.1F)) return false;
        if (CC.HasCooldown(Constants.ActTraumatizeId)) return false;
        if (Act.TC == null) return false;
        return true;
    }
    
    // Ability that costs mana.
    public override Cost GetCost(Chara c)
    {
        Cost cost = base.GetCost(c);
        cost.type = CostType.MP;
        return cost;
    }

    public override bool Perform()
    {
        List<Condition> negativeConditions = TC.Chara.conditions.Where(con => con.Type is ConditionType.Bad or ConditionType.Debuff).ToList();
        if (negativeConditions.Count == 0)
        {
            CC.SayNothingHappans();
        }
        else
        {
            int damage = HelperFunctions.SafeDice("TraumatizeAlias", this.GetPower(CC));
            damage = HelperFunctions.SafeMultiplier(damage, negativeConditions.Count);
            foreach (Condition con in negativeConditions) con.Kill();
            TC.DamageHP(damage, Constants.EleMind, 100, AttackSource.None, CC);
        }
        CC.AddCooldown(Constants.ActTraumatizeId, 10);
        return true;
    }
}