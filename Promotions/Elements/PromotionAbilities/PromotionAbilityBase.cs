using PromotionMod.Common;

namespace PromotionMod.Elements.PromotionAbilities;

public class PromotionAbilityBase : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatLuminary) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.LuminaryId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActLightWaveId)) return false;
        return base.CanPerform();
    }
}