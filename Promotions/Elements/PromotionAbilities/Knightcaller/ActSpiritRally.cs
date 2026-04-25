using PromotionMod.Common;
using PromotionMod.Stats.Knightcaller;
namespace PromotionMod.Elements.PromotionAbilities.Knightcaller;

public class ActSpiritRally : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatKnightcaller;
    public override string PromotionString => Constants.KnightcallerId;
    public override int AbilityId => Constants.ActSpiritRallyId;

    public override bool Perform()
    {
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 20F, CC, true, true))
        {
            if (target.IsPCPartyMinion)
            {
                target.Teleport(CC.pos.GetNearestPoint(false, false) ?? CC.pos);
                int healthHealAmount = (int)(target.MaxHP * 0.5F);
                int manaHealAmount = (int)(target.mana.max * 0.5F);
                int staminaHealAmount = (int)(target.stamina.max * 0.5F);
                target.HealHP(healthHealAmount);
                target.mana.Mod(manaHealAmount);
                target.stamina.Mod(staminaHealAmount);
                target.AddCondition<ConSpiritRally>();
            }
        }

        CC.AddCondition<ConSpiritRally>(GetPower(CC));
        return true;
    }
}