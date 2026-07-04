using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements;

/// <summary>
///     Alraune Feat
///     During the Day, the character will automatically cast Nature's Embrace on themselves for free.
///     While the Alraune is wet and in the sunlight, they will undergo photosynthesis which increases their rate of hunger
///     gain.
/// </summary>
public class FeatAlraune : Feat
{
    public override List<string> Apply(int a, ElementContainer owner, bool hint = false)
    {
        if (hint) return base.Apply(a, owner, hint);
        
        if (a >= 1)
        {
            if (owner.Chara.IsPC)
            {
                if (!owner.Chara.HasElement(Constants.StSweetScentId))
                {
                    owner.Chara.elements.SetBase(Constants.StSweetScentId, 1);
                }
                if (!owner.Chara.HasElement(Constants.ActWildGrowthId))
                {
                    owner.Chara.elements.SetBase(Constants.ActWildGrowthId, 1);
                }
                if (!owner.Chara.HasElement(Constants.ActAlrauneConsumeId))
                {
                    owner.Chara.elements.SetBase(Constants.ActAlrauneConsumeId, 1);
                }
            }
            else
            {
                if (!owner.Chara.ability.Has(Constants.ActAlrauneConsumeId)) owner.Chara.ability.Add(Constants.ActAlrauneConsumeId, 100, false);
                if (!owner.Chara.ability.Has(Constants.StSweetScentId)) owner.Chara.ability.Add(Constants.StSweetScentId, 100, false);
                if (!owner.Chara.ability.Has(Constants.ActWildGrowthId)) owner.Chara.ability.Add(Constants.ActWildGrowthId, 75, false);
            }
        }
        else
        {
            if (owner.Chara.IsPC)
            {
                owner.Chara.elements.Remove(Constants.StSweetScentId);
                owner.Chara.elements.Remove(Constants.ActWildGrowthId);
                owner.Chara.elements.Remove(Constants.ActAlrauneConsumeId);
            }
            else
            {
                if (owner.Chara.ability.Has(Constants.StSweetScentId)) owner.Chara.ability.Remove(Constants.StSweetScentId);
                if (owner.Chara.ability.Has(Constants.ActWildGrowthId)) owner.Chara.ability.Remove(Constants.ActWildGrowthId);
                if (owner.Chara.ability.Has(Constants.ActAlrauneConsumeId)) owner.Chara.ability.Remove(Constants.ActAlrauneConsumeId);
            }
        }
        
        return base.Apply(a, owner, hint);
    }
}