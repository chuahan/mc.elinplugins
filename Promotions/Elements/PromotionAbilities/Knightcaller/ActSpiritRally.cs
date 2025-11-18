using PromotionMod.Common;
using PromotionMod.Stats.Knightcaller;
namespace PromotionMod.Elements.PromotionAbilities.Knightcaller;

public class ActSpiritRally : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatKnightcaller) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.KnightcallerId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActSpiritRallyId)) return false;
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
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 10F, CC, true, true))
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

        CC.AddCooldown(Constants.ActSpiritRallyId, 10);
        return true;
    }
}