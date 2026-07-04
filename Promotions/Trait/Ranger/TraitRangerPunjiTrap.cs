
using PromotionMod.Common;
using PromotionMod.Patches;
namespace PromotionMod.Trait.Ranger;

public class TraitRangerPunjiTrap : TraitFactionTrap
{
    public override string TrapName => Constants.RangerPunjiTrapAlias;

    public override void ActivateTrapInternal(Chara c)
    {
        HelperFunctions.DamageHpWrapper(c, GetPower(), Constants.EleCut, 100, AttackSource.Trap, null);
        c.PlayEffect("hit_slash");
        c.AddCondition<ConBleed>(GetPower(), true);
        ActEffect.Proc(EffectId.DebuffStats, c, null, GetPower(), new ActRef
        {
            n1 = "SPD"
        });
    }
}