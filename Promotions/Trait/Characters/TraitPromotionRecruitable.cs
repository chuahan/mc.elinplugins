namespace PromotionMod.Trait.Characters;

public class TraitPromotionRecruitable : TraitPromotionUnrecruitable
{
    public override bool RecruitmentCondition => player.dialogFlags.TryGetValue(owner.Chara.id + "Recruitable") > 0;
    
    public override bool CanInvite => RecruitmentCondition;
    public override bool CanJoinParty => RecruitmentCondition;
    public override bool CanJoinPartyResident => RecruitmentCondition;
    public override bool CanBout => false;
}