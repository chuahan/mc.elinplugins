using PromotionMod.Common;
namespace PromotionMod.Trait.Ranger;

public class TraitRangerPoisonMine : TraitFactionTrap
{
    public override string TrapName => Constants.RangerPoisonTrapAlias;

    public override void OnActivateTrap(Chara c)
    {
        c.PlaySound("trap");
        Msg.Say(TrapName.langGame(), c);

        c.AddCondition<ConPoison>(GetPower(), true);
        c.PlayEffect("smoke");
    }
}