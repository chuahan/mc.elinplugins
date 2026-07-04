namespace NierMod.Traits;

internal class TraitNier : TraitUniqueChara
{
    private static bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("nierBefriended") >= 1 ||
                                                     player.dialogFlags.TryGetValue("nierFriend") >= 1 ||
                                                     player.dialogFlags.TryGetValue("nierMarried") >= 1;

    public override bool CanInvite => IsBefriendedThroughDialog;
    public override bool CanJoinParty => IsBefriendedThroughDialog;
    public override bool CanJoinPartyResident => IsBefriendedThroughDialog;
    public override bool CanBout => false;
}