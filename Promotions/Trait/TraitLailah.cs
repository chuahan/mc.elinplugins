namespace PromotionMod.Trait;

public class TraitLailah : TraitUniqueChara
{
    public static bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("lailahRecruited") > 0;

    public override bool CanInvite => IsBefriendedThroughDialog;
    public override bool CanJoinParty => IsBefriendedThroughDialog;
    public override bool CanJoinPartyResident => IsBefriendedThroughDialog;
    public override bool CanBout => false;
}