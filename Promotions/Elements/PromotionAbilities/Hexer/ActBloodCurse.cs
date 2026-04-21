using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
namespace PromotionMod.Elements.PromotionAbilities.Hexer;

// Force applies one of the curses randomly at the cost of 10% life. Will prioritize curses you have not applied of the same tier that you roll.
public class ActBloodCurse : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatHexer;
    public override string PromotionString => Constants.HexerId;
    public override int AbilityId => Constants.ActBloodCurseId;
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;

    public override bool CanPerformExtra()
    {
        if (CC.hp <= CC.MaxHP * 0.1F) return false;
        if (TC is not { isChara: true }) return false;
        return true;
    }

    public override bool Perform()
    {
        FeatHexer.ApplyCondition(TC.Chara, CC, GetPower(CC), true);
        int hpCost = (int)(CC.MaxHP * 0.1F);
        CC.hp -= hpCost;
        return true;
    }
}