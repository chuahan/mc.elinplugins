using System.Collections.Generic;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Elements;

public class FeatMartialArtist : PromotionFeat
{
    public override string PromotionClassId => Constants.MartialArtistId;
    public override int PromotionClassFeatId => Constants.FeatMartialArtist;

    public override List<int> PromotionAbilities => new List<int>
    {
    };

    public override string JobRequirement => "";

    protected override bool Requirement()
    {
        return pc.GetFlagValue(Constants.MartialArtistPromotionUnlockedFlag) > 0;
    }

    protected override void ApplyInternalNPC(Chara c)
    {
        //c.ability.Add(Constants.ActCardThrowId, 75, false);
        //c.ability.Add(Constants.ActLuckyCatId, 75, true);
    }

    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add, eleOwner, hint);
    }
}