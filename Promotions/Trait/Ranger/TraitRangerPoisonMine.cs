using PromotionMod.Common;
namespace PromotionMod.Trait.Ranger;

public class TraitRangerPoisonMine : TraitFactionTrap
{
    public override string TrapName => Constants.RangerPoisonTrapAlias;

    public override void ActivateTrapInternal(Chara c)
    {
        c.AddCondition<ConPoison>(GetPower(), true);
        c.PlayEffect("smoke");
    }
}