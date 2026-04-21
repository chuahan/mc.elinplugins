using PromotionMod.Common;
using PromotionMod.Stats.Sentinel;
namespace PromotionMod.Elements.PromotionAbilities.Sentinel;

public class ActShout : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatSentinel;
    public override string PromotionString => Constants.SentinelId;
    public override int Cooldown => 5;
    public override int AbilityId => Constants.ActShoutId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool Perform()
    {
        CC.PlaySound("warcry");
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true))
        {
            ConTaunted taunted = target.AddCondition<ConTaunted>(force: true) as ConTaunted;
            taunted.TaunterUID = CC.uid;
        }

        CC.AddCooldown(AbilityId, Cooldown);
        CC.TalkRaw("sentinelTaunt".langList().RandomItem());
        return true;
    }
}