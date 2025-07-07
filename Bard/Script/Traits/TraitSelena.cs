namespace NierMod.Traits;

internal class TraitSelena : TraitUniqueChara
{
    public static bool IsBefriendedThroughDialog => EClass.player.dialogFlags.TryGetValue("selenaRecruited", 0) > 0;

    public override bool CanInvite => IsBefriendedThroughDialog;
    public override bool CanJoinParty => IsBefriendedThroughDialog;
    public override bool CanJoinPartyResident => IsBefriendedThroughDialog;
    public override bool CanBout => false;
}