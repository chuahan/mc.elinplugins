using PromotionMod.Common;
namespace PromotionMod.Trait.Ranger;

public class TraitRangerParalyticMine : TraitFactionTrap
{
    public override string TrapName => Constants.RangerParalyzeTrapAlias;

    public override void OnActivateTrap(Chara c)
    {
        c.PlaySound("trap");
        Msg.Say(TrapName.langGame(), c);

        c.AddCondition<ConParalyze>(GetPower(), true);
        c.PlayEffect("smoke");
    }
}