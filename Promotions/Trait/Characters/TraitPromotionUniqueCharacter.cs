using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitPromotionUniqueCharacter : TraitUniqueChara
{
    public virtual bool IsBefriendedThroughDialog => false;

    public virtual int Prepromotion => -1;
    public override bool CanInvite => IsBefriendedThroughDialog;
    public override bool CanJoinParty => IsBefriendedThroughDialog;
    public override bool CanJoinPartyResident => IsBefriendedThroughDialog;
    public override bool CanBout => false;
    public override bool CanWhore => false;
    public override bool CanGiveRandomQuest => false;

    public override void OnAddedToZone()
    {
        base.OnAddedToZone();
        if (this.Prepromotion != -1)
        {
            if (owner.GetFlagValue(Constants.PromotionFeatFlag) == 0 && owner.HasElement(Prepromotion))
            {
                // Load the default Prepromotion so they can use their abilities.
                owner.SetFlagValue(Constants.PromotionFeatFlag, Prepromotion);
            }
        }
    }
}