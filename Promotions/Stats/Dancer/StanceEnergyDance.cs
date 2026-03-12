namespace PromotionMod.Stats.Dancer;

public class StanceEnergyDance : StanceDance
{
    public override void ActInternal(Chara target, int dancePower, bool isPartner)
    {
        int energyDancePower = isPartner ? 2 : 1;
        Condition? danceBuff = target.GetCondition<ConEnergyDance>() ?? target.AddCondition<ConEnergyDance>(energyDancePower);
        if (danceBuff is { value: >= 3 }) return;
        danceBuff?.Mod(1);
    }
}