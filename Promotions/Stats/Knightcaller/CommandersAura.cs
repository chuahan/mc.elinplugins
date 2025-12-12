namespace PromotionMod.Stats.Knightcaller;

public class CommandersAura : ConAura
{
    public override bool CanManualRemove => true;
    public override bool FriendlyAura => true;
    public override int AuraRadius => 5;

    public override void ApplyInternal(Chara target)
    {
        target.AddCondition<ConKnightlySpirit>();
    }
}