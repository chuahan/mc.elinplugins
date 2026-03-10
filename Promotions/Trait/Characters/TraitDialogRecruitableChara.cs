namespace PromotionMod.Trait.Characters;

public class TraitDialogRecruitableChara : TraitUniqueChara
{
    public virtual bool IsBefriendedThroughDialog => false;

    public override bool CanInvite => IsBefriendedThroughDialog;
    public override bool CanJoinParty => IsBefriendedThroughDialog;
    public override bool CanJoinPartyResident => IsBefriendedThroughDialog;
    public override bool CanBout => false;
    public override bool CanWhore => false;
    public override bool CanGiveRandomQuest => false;
}