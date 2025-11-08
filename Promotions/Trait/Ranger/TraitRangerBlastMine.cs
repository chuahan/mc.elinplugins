using PromotionMod.Common;
namespace PromotionMod.Trait.Ranger;

public class TraitRangerBlastMine : TraitFactionTrap
{
    public override string TrapName => Constants.RangerBlastTrapAlias;

    public override void OnActivateTrap(Chara c)
    {
        c.PlaySound("trap");
        Msg.Say(TrapName.langGame(), c);

        c.DamageHP(GetPower(), AttackSource.Trap);
        c.AddCondition<ConDim>(GetPower(), true);
        c.pos.PlayEffect("explosion");
        c.TryMoveFrom(owner.pos);
    }
}