namespace PromotionMod.Stats.HolyKnight;

public class RadiantAura : ConAura
{
    public override bool CanManualRemove => false;
    public override AuraType AuraTarget => AuraType.Both;
    public override void ApplyFriendly(Chara target)
    {
        // Protect Allies
    }
    public override void ApplyFoe(Chara target)
    {
        // Deal Holy Damage + Stun. Double on Undead.
    }
}