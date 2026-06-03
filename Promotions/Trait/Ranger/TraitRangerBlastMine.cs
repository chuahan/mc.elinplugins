using Cwl.Helper;
using PromotionMod.Common;
using PromotionMod.Patches;
namespace PromotionMod.Trait.Ranger;

public class TraitRangerBlastMine : TraitFactionTrap
{
    public override string TrapName => Constants.RangerBlastTrapAlias;

    public override void ActivateTrapInternal(Chara c)
    {
        HelperFunctions.DamageHpWrapper(c, GetPower(), Constants.EleImpact, 100, AttackSource.Trap, null);
        c.AddCondition<ConDim>(GetPower(), true);
        c.pos.PlayEffect("explosion");
        c.TryMoveFrom(owner.pos);
    }
}