using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Sentinel;

/// <summary>
/// Sentinel Ability
/// Apply Taunt to nearby enemies.
/// </summary>
public class ActShout : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSentinel) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SentinelId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActShoutId)) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true))
        {
            target.AddCondition<ConTaunted>(this.GetPower(CC));
        }

        CC.AddCooldown(Constants.ActShoutId, 5);
        CC.TalkRaw("sentinelTaunt".langList().RandomItem());
        return true;
    }
}