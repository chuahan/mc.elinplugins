using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.Abilities.Dancer;

public class StanceHealingDance : StanceDance
{
    public override void ActInternal(Chara target, int dancePower, bool isPartner)
    {
        // Heals 10% HP a turn.
        owner.HealHP((int)(target.MaxHP * 0.1F), HealSource.HOT);
    }
}