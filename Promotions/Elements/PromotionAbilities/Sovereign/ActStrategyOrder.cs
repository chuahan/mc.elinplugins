using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Sovereign;

namespace PromotionMod.Elements.PromotionAbilities.Sovereign;

public class ActStrategyOrder : ActSovereignOrder
{
    protected override string OrderType => "strategy";
    protected override int CooldownId => Constants.ActStrategyOrderId;
    public override void AddLawCondition(Chara chara, int stacks)
    {
        // Burst Heal Allies and provide protection and regen.
        int healingAmount = HelperFunctions.SafeDice("sovereign_rally", this.GetPower(CC));
        healingAmount *= stacks;
        chara.HealHP(healingAmount, HealSource.Magic);
        chara.AddCondition<ConProtection>(ConProtection.CalcProtectionAmount(healingAmount));
        chara.AddCondition<ConOrderRally>(stacks);
    }
    public override void AddChaosCondition(Chara chara, int stacks)
    {
        ConWeapon holyIntonation = new ConWeapon();
        holyIntonation.SetElement(Constants.EleHoly);
        holyIntonation.power = this.GetPower(CC);
        chara.AddCondition(holyIntonation);
        chara.AddCondition<ConOrderRout>(stacks);
    }
}