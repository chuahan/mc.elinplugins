using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.Abilities.Dancer;

public class StanceMistDance : StanceDance
{
    public override void ActInternal(Chara target, int power, bool isPartner)
    {
        Condition? danceBuff = target.GetCondition<ConMistDance>() ?? target.AddCondition<ConMistDance>();
        if (danceBuff is { value: >= 3 }) return;
        danceBuff?.Mod(1);
    }
}