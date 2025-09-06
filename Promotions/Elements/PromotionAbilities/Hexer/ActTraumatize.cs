using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Hexer;

/// <summary>
/// Does Mind Damage scaling on many negative conditions are on the enemy. 5 turn cooldown. 20% Mana cost.
/// Does not consume the negative conditions like Trickster or Shatter Hex. 
/// </summary>
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
    
    public override Cost GetCost(Chara c)
    {
        return new Cost()
        {
            type = CostType.MP,
            cost = (int)(c.mana.max * 0.2F),
        };
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
            int damage = HelperFunctions.SafeDice(Constants.TraumatizeAlias, this.GetPower(CC));
            damage = HelperFunctions.SafeMultiplier(damage, negativeConditions.Count);
            TC.DamageHP(damage, Constants.EleMind, 100, AttackSource.None, CC);
        }
        CC.AddCooldown(Constants.ActTraumatizeId, 5);
        return true;
    }
}