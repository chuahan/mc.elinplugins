namespace PromotionMod.Trait;

public class TraitHarbinger : TraitUniqueChara
{
    public override bool CanInvite => false;
    public override bool CanJoinParty => false;
    public override bool CanJoinPartyResident => false;
}