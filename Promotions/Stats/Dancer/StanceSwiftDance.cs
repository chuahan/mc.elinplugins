using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.Abilities.Dancer;

public class StanceSwiftDance : StanceDance
{
    public override void ActInternal(Chara target, int dancePower, bool isPartner)
    {
        int buffPower = isPartner ? 15 : 10;
        Condition? danceBuff = target.GetCondition<ConSwiftDance>() ?? target.AddCondition<ConSwiftDance>(buffPower);
        if (danceBuff is { value: >= 3 }) return;
        danceBuff?.Mod(1);
    }
}