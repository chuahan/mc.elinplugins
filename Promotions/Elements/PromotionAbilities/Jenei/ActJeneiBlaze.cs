using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiBlaze : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJenei) == 0) return false;
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
        int power = GetPower(CC);
        int damage = HelperFunctions.SafeDice("jenei_blaze", power);
        HelperFunctions.ProcSpellDamage(power, damage, CC, TC.Chara, ele: Constants.EleFire, eleP: 150);
        TC.Chara.AddCondition<ConBurning>(GetPower(CC), true);
        TC.pos.PlayEffect("ball_Fire");
        _map.ModFire(TC.pos.x, TC.pos.z, 10);
        return true;
    }
}