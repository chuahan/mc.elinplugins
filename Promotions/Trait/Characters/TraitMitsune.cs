namespace PromotionMod.Trait.Characters;

public class TraitMitsune : TraitDialogRecruitableChara
{
    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("louiseRecruited") > 0;
}