using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
namespace PromotionMod.Stats.Jenei;

public class ConJenei : ClassCondition
{
    [JsonProperty(PropertyName = "Q")] public Queue<int> ElementalQueue = new Queue<int>();

    public void AddElement(int element)
    {
        if (ElementalQueue.Count >= 13)
        {
            ElementalQueue.Dequeue();
        }
        ElementalQueue.Enqueue(element);
    }

    public Dictionary<int, int> GetElementalStockpile()
    {
        return ElementalQueue
                .GroupBy(e => e)
                .ToDictionary(g => g.Key, g => g.Count());
    }

    public void EmptyStockpile()
    {
        ElementalQueue.Clear();
        Owner.Say("jenei_recovery".langGame(owner.NameSimple));
    }

    public override void OnWriteNote(List<string> list)
    {
        Dictionary<int, int> stockpile = GetElementalStockpile();
        string? readySummon = FeatJenei.JeneiSummons.GetSummon(stockpile);
        list.Add("hintJeneiVenus".lang(stockpile[Constants.EleImpact].ToString()));
        list.Add("hintJeneiMars".lang(stockpile[Constants.EleImpact].ToString()));
        list.Add("hintJeneiJupiter".lang(stockpile[Constants.EleImpact].ToString()));
        list.Add("hintJeneiMercury".lang(stockpile[Constants.EleImpact].ToString()));
        if (readySummon != null)
        {
            string summonName = readySummon + "_formalname";
            list.Add("hintJeneiSummonReady".lang(summonName.langGame()));
        }
    }
}