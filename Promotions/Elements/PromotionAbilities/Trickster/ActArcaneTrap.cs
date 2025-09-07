using PromotionMod.Common;
using PromotionMod.Trait;
namespace PromotionMod.Elements.PromotionAbilities.Trickster;

public class ActArcaneTrap : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatTrickster) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.TricksterId.lang()));
            return false;
        }
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
        Thing trap = ThingGen.Create(Constants.TricksterArcaneTrapAlias);
        Zone.ignoreSpawnAnime = true;
        _zone.AddCard(trap, TP);
        (trap.trait as TraitFactionTrap)?.OnInstall(CC.IsPCPartyMinion);
        return true;
    }
}