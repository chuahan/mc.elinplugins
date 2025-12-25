namespace PromotionMod.Stats.Knightcaller;

public class CommandersAura : ConAura
{
    public override bool CanManualRemove => true;
    
    public override int AuraRadius => 5;

    public override void ApplyFriendly(Chara target)
    {
        target.AddCondition<ConKnightlySpirit>();
    }
}