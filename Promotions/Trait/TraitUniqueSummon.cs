namespace PromotionMod.Trait;

internal class TraitUniqueSummon : TraitUniqueChara
{
    public override bool CanInvite => false;
    public override bool CanJoinParty => false;
    public override bool CanJoinPartyResident => false;
}