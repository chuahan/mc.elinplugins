using PromotionMod.Common;
namespace PromotionMod.Elements;

public class StFeelingLucky : PromotionAbility
{
    public override int PromotionId => Constants.FeatGambler;
    public override string PromotionString => Constants.GamblerId;
    public override int AbilityId => Constants.StFeelingLuckyId;
}