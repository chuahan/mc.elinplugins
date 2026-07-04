using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements;

/// <summary>
///     The Gambler
/// </summary>
public class FeatGambler : PromotionFeat
{
    public override string PromotionClassId => Constants.GamblerId;
    public override int PromotionClassFeatId => Constants.FeatGambler;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActCardThrowId,
        Constants.ActDiceStrikeId,
        Constants.ActLuckyCatId,
        Constants.ActSpinSlotsId,
        Constants.StFeelingLuckyId
    };

    public override string JobRequirement => "";

    protected override bool Requirement()
    {
        return pc.GetBool(Constants.GamblerPromotionUnlockedFlag);
    }

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActCardThrowId, 75, false);
        c.ability.Add(Constants.ActDiceStrikeId, 75, false);
        c.ability.Add(Constants.ActLuckyCatId, 75, true);
        c.ability.Add(Constants.ActSpinSlotsId, 25, false);
        c.ability.Add(Constants.StFeelingLuckyId, 100, false);
    }


}