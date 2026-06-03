namespace NierMod.Traits;

internal class TraitNier : TraitUniqueChara
{
    public static bool IsBefriendedThroughDialog => (
        (EClass.player.dialogFlags.TryGetValue("nierBefriended", 0) >= 1) ||
        ((EClass.player.dialogFlags.TryGetValue("nierFriend", 0) >= 1) ||
        (EClass.player.dialogFlags.TryGetValue("nierMarried", 0) >= 1)));

    public override bool CanInvite => IsBefriendedThroughDialog;
    public override bool CanJoinParty => IsBefriendedThroughDialog;
    public override bool CanJoinPartyResident => IsBefriendedThroughDialog;
    public override bool CanBout => false;
}