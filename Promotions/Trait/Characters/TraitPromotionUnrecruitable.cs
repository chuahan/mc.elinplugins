namespace PromotionMod.Trait.Characters;

public class TraitPromotionUnrecruitable : TraitUniqueChara
{
    // If False, cannot be recruited.
    public virtual bool RecruitmentCondition => false;
    
    public override bool CanInvite => RecruitmentCondition;
    public override bool CanJoinParty => RecruitmentCondition;
    public override bool CanJoinPartyResident => RecruitmentCondition;
    public override bool CanBout => false;
}