using Cwl.Helper.Extensions;
namespace PromotionMod.Common;

public static class PromoCharaExtensions
{
    public static bool MatchesPromotion(this Chara target, int promotedId)
    {
        if (target == null) return false;
        return target.GetFlagValue(Constants.PromotionFeatFlag) == promotedId;
    }
}