using PromotionMod.Common;

namespace PromotionMod.Elements.PromotionAbilities.WitchHunter;

public class ActManaBreak : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatWitchHunter) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.WitchHunterId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActLightWaveId)) return false;
        return base.CanPerform();
    }
}