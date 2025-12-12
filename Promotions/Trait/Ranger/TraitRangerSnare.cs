using PromotionMod.Common;
namespace PromotionMod.Trait.Ranger;

public class TraitRangerSnare : TraitFactionTrap
{
    public override string TrapName => Constants.RangerSnareTrapAlias;

    public override void ActivateTrapInternal(Chara c)
    {
        c.AddCondition<ConEntangle>(GetPower(), true);
    }
}