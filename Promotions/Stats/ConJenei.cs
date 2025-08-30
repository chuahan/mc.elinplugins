using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
namespace PromotionMod.Stats;

public class ConJenei : ClassCondition
{
    [JsonProperty(PropertyName = "E")] public Dictionary<int, int> ElementalStockpile = new Dictionary<int, int>
    {
        {
            Constants.EleFire, 0
        },
        {
            Constants.EleCold, 0
        },
        {
            Constants.EleLightning, 0
        },
        {
            Constants.EleImpact, 0
        }
    };

    public void EmptyStockpile()
    {
        ElementalStockpile[Constants.EleFire] = 0;
        ElementalStockpile[Constants.EleCold] = 0;
        ElementalStockpile[Constants.EleLightning] = 0;
        ElementalStockpile[Constants.EleImpact] = 0;
        Owner.Say("jenei_recovery".langGame(owner.NameSimple));
    }
    
    public override void OnWriteNote(List<string> list)
    {
        string? readySummon = FeatJenei.JeneiSummons.GetSummon(ElementalStockpile);
        list.Add("hintJeneiVenus".lang(ElementalStockpile[Constants.EleImpact].ToString()));
        list.Add("hintJeneiMars".lang(ElementalStockpile[Constants.EleImpact].ToString()));
        list.Add("hintJeneiJupiter".lang(ElementalStockpile[Constants.EleImpact].ToString()));
        list.Add("hintJeneiMercury".lang(ElementalStockpile[Constants.EleImpact].ToString()));
        if (readySummon != null)
        {
            string summonName = readySummon + "_formalname";
            list.Add("hintJeneiSummonReady".lang(summonName.langGame()));
        }
    }
}