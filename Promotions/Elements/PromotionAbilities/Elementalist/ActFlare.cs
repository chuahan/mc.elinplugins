using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Elementalist;

/// <summary>
///     Elementalist Ability
///     Consumes all elemental orbs and does Void-typed damage with it to a single target.
/// </summary>
public class ActFlare : Ability
{
    public int FlareRequirement = 5;
    public override Cost GetCost(Chara c)
    {
        Cost cost = base.GetCost(c);
        cost.type = CostType.MP;
        return cost;
    }

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatElementalist) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.ElementalistId.lang()));
            return false;
        }
        if (CC.HasCondition<ConElementalist>())
        {
            ConElementalist elementalistStockpile = CC.GetCondition<ConElementalist>();
            if (elementalistStockpile.GetElementalStrength() < FlareRequirement) return false;
        }
        else
        {
            return false;
        }

        return base.CanPerform();
    }

    public override bool Perform()
    {
        int damage = HelperFunctions.SafeDice(Constants.FlareAlias, GetPower(CC));
        ConElementalist elementalistStockpile = CC.GetCondition<ConElementalist>();
        int elementalBoostedDamage = HelperFunctions.SafeMultiplier(damage, elementalistStockpile.GetElementalStrength());

        // PLAY SFX
        TC.PlaySound("explosion");
        TC.PlayEffect("explosion");
        TC.DamageHP(elementalBoostedDamage, Constants.EleVoid, 100, AttackSource.None, CC);
        elementalistStockpile.ConsumeElementalOrbs();
        return true;
    }
}