using PromotionMod.Common;
using PromotionMod.Stats.WitchHunter;
namespace PromotionMod.Elements.PromotionAbilities.WitchHunter;

public class ActMagicReflect : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatWitchHunter;
    public override string PromotionString => Constants.WitchHunterId;
    public override int AbilityId => Constants.ActMagicReflectId;
    public override int Cooldown => 5;
    
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool Perform()
    {
        CC.Chara.AddCondition<ConMagicReflect>(GetPower(CC));
        CC.AddCooldown(AbilityId, Cooldown);
        return true;
    }
}