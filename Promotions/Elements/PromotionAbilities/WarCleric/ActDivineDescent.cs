using PromotionMod.Common;

namespace PromotionMod.Elements.PromotionAbilities.WarCleric;

public class ActDivineDescent : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatWarCleric) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.WarClericId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActLightWaveId)) return false;
        return base.CanPerform();
    }
}