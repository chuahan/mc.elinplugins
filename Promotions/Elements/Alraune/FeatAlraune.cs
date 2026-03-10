using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Elements.Alraune;

/// <summary>
/// Alraune Feat
/// During the Day, the character will automatically cast Nature's Embrace on themselves for free.
/// While the Alraune is wet and in the sunlight, they will undergo photosynthesis which increases their rate of hunger gain.
/// </summary>
public class FeatAlraune : Feat
{
    virtual internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        if (hint) return;
        if (owner.Chara.IsPC)
        {
            if (!owner.Chara.HasElement(Constants.StSweetScentId))
            {
                owner.Chara.AddElement(Constants.StSweetScentId);
            }
            if (!owner.Chara.HasElement(Constants.ActWildGrowthId))
            {
                owner.Chara.AddElement(Constants.ActWildGrowthId);
            }
        }
        else
        {
            if (!owner.Chara.ability.Has(Constants.StSweetScentId)) owner.Chara.ability.Add(Constants.StSweetScentId, 100, false);
            if (!owner.Chara.ability.Has(Constants.ActWildGrowthId)) owner.Chara.ability.Add(Constants.ActWildGrowthId, 75, false);
        }
    }
}