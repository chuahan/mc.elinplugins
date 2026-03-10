using PromotionMod.Common;
namespace PromotionMod.Trait.Ranger;

public class TraitRangerPunjiTrap : TraitFactionTrap
{
    public override string TrapName => Constants.RangerPunjiTrapAlias;

    public override void ActivateTrapInternal(Chara c)
    {
        c.DamageHP(GetPower(), AttackSource.Trap);
        c.PlayEffect("hit_slash");
        c.AddCondition<ConBleed>(GetPower(), true);
        ActEffect.ProcAt(EffectId.DebuffStats, GetPower(), BlessedState.Normal, Act.CC, c, c.pos, true, new ActRef
        {
            origin = Act.CC.Chara,
            n1 = "SPD"
        });
    }
}