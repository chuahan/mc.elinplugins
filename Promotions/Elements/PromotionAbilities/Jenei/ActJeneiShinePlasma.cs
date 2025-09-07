using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiShinePlasma : Ability
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
        int damage = HelperFunctions.SafeDice("jenei_shineplasma", power);
        HelperFunctions.ProcSpellDamage(power, damage, CC, TC.Chara, ele: Constants.EleLightning);
        TC.Chara.AddCondition<ConWet>(GetPower(CC), true);
        TC.pos.PlayEffect("attack_lightning");
        return true;
    }
}