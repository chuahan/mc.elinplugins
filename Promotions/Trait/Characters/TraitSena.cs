namespace PromotionMod.Trait.Characters;

public class TraitSena : TraitDialogRecruitableChara
{
    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("louiseRecruited") > 0;
}