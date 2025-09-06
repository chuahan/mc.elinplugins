namespace PromotionMod.Stats.WitchHunter;

public class ConNullZone : BaseBuff
{
    public override bool TimeBased => true;
    public override bool CanManualRemove => true;

    public override void Tick()
    {
        // Apply Sanctuary to everyone within 3F.
        foreach (Chara chara in EClass.pc.currentZone.map.ListCharasInCircle(owner.pos, 3F, true)) {
            var sanctuary  = chara.GetCondition<ConNullPresence>() ?? chara.AddCondition<ConNullPresence>();
            if (sanctuary is { value: > 1 }) {
                continue;
            }

            sanctuary?.Mod(1);
        }
        base.Tick();
    }
}