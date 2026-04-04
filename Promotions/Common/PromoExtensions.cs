using Cwl.Helper.Extensions;
namespace PromotionMod.Common;

public static class PromoCharaExtensions
{
    public static bool MatchesPromotion(this Chara target, int promotedId)
    {
        if (target == null) return false;
        return target.GetFlagValue(Constants.PromotionFeatFlag) == promotedId;
    }

    public static Point GetRoomCenter(this Room target)
    {
        int averageX = (target.pointMaxX.x + target.pointMinX.x) / 2;
        int averageZ = (target.pointMaxX.z + target.pointMinX.z) / 2;
        return new Point(averageX, averageZ);
    }
}