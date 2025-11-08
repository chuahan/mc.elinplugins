namespace PromotionMod.Trait.Artificer;

public class TraitArtificerGolem : TraitUniqueChara
{
    public override bool CanInvite => true;
    public override bool CanJoinParty => true;
    public override bool CanJoinPartyResident => true;

    public void ApplyPreceptChip(Thing golemChip)
    {
        
    }
}