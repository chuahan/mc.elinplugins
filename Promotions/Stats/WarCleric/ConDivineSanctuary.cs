namespace PromotionMod.Stats.WarCleric;

public class ConDivineSanctuary : BaseBuff
{
    public override bool TimeBased => true;
    public override bool CanManualRemove => true;

    public override void Tick()
    {
        // Apply Sanctuary to everyone within 3F.
        foreach (Chara chara in pc.currentZone.map.ListCharasInCircle(owner.pos, 3F))
        {
            Condition? sanctuary = chara.GetCondition<ConSanctuary>() ?? chara.AddCondition<ConSanctuary>();
            if (sanctuary is { value: > 1 })
            {
                continue;
            }

            sanctuary?.Mod(1);
        }
        base.Tick();
    }
}