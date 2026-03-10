namespace PromotionMod.Trait.Characters;

public class TraitLouise : TraitDialogRecruitableChara
{
    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("louiseRecruited") > 0;
}