using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Trickster;

public class ActArcaneTrap : Ability
{
    public override int PerformDistance => 5;

    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatTrickster))
        {
            Msg.Say("classlocked_ability".lang(Constants.TricksterId.lang()));
            return false;
        }
        // Cannot stack traps or place in pc faction
        if (TP.Installed != null || _zone.IsPCFaction) return false;
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
        Thing trap = ThingGen.Create(Constants.TricksterArcaneTrapAlias, -1, GetPower(CC));
        if (CC.IsPCFaction) trap.SetFlagValue(Constants.IsPlayerFactionTrapFlag);
        Zone.ignoreSpawnAnime = true;
        _zone.AddCard(trap, TP).Install();
        return true;
    }
}