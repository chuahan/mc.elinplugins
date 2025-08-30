using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.Abilities.Dancer;

public class StanceSwiftDance : StanceDance
{
    public override void ActInternal(Chara target, int dancePower, bool isPartner)
    {
        Condition? danceBuff = target.GetCondition<ConSwiftDance>() ?? target.AddCondition<ConSwiftDance>();
        if (danceBuff is { value: >= 3 }) return;
        danceBuff?.Mod(1);
    }
}