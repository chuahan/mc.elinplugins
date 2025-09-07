using PromotionMod.Common;
namespace PromotionMod.Trait.Ranger;

public class TraitRangerSnare : TraitFactionTrap
{
    public override string TrapName => Constants.RangerSnareTrapAlias;

    public override void OnActivateTrap(Chara c)
    {
        c.PlaySound("trap");
        Msg.Say(TrapName.langGame(), c);

        c.AddCondition<ConEntangle>(owner.LV);
    }
}