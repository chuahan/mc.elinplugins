namespace PromotionMod.Trait.Characters;

public class TraitRuras : TraitDialogRecruitableChara
{
    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("louiseRecruited") > 0;
}