using PromotionMod.Common;

namespace PromotionMod.Elements.PromotionAbilities.Trickster;

public class ActReversal : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatTrickster) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.TricksterId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActLightWaveId)) return false;
        return base.CanPerform();
    }
}