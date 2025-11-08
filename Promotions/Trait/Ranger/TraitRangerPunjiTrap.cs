using PromotionMod.Common;
namespace PromotionMod.Trait.Ranger;

public class TraitRangerPunjiTrap : TraitFactionTrap
{
    public override string TrapName => Constants.RangerPunjiTrapAlias;

    public override void OnActivateTrap(Chara c)
    {
        c.PlaySound("trap");
        Msg.Say(TrapName.langGame(), c);

        c.DamageHP(GetPower(), AttackSource.Trap);
        c.PlayEffect("hit_slash");
        c.AddCondition<ConBleed>(GetPower(), true);
    }
}