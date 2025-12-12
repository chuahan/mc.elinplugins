using PromotionMod.Common;
namespace PromotionMod.Trait.Ranger;

public class TraitRangerParalyticMine : TraitFactionTrap
{
    public override string TrapName => Constants.RangerParalyzeTrapAlias;

    public override void ActivateTrapInternal(Chara c)
    {
        c.AddCondition<ConParalyze>(GetPower(), true);
        c.PlayEffect("smoke");
    }
}