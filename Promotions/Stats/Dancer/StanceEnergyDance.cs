using PromotionMod.Elements.Abilities.Dancer;
namespace PromotionMod.Stats.Dancer;

public class StanceEnergyDance : StanceDance
{
    public override void ActInternal(Chara target, int power, bool isPartner)
    {
        Condition? danceBuff = target.GetCondition<ConEnergyDance>() ?? target.AddCondition<ConEnergyDance>();
        if (danceBuff is { value: >= 3 }) return;
        danceBuff?.Mod(1);
    }
}