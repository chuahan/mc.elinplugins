using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Berserker;

public class ActLifebreak : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatBerserker) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.BerserkerId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActLifebreakId)) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 5,
            type = CostType.SP
        };
    }

    public override bool Perform()
    {
        int damage = CC.Chara.MaxHP - CC.Chara.hp;
        damage = HelperFunctions.SafeMultiplier(damage, 1.3F);
        TC.DamageHP(damage, AttackSource.Melee, CC);
        CC.AddCooldown(Constants.ActLifebreakId, 10);
        return true;
    }
}