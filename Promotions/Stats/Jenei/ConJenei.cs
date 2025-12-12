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
        list.Add("hintJenei".lang());
        if (stockpile.TryGetValue(Constants.EleImpact, out int earthCount)) list.Add("hintJeneiVenus".lang(earthCount.ToString()));
        if (stockpile.TryGetValue(Constants.EleFire, out int fireCount)) list.Add("hintJeneiMars".lang(fireCount.ToString()));
        if (stockpile.TryGetValue(Constants.EleLightning, out int windCount)) list.Add("hintJeneiJupiter".lang(windCount.ToString()));
        if (stockpile.TryGetValue(Constants.EleCold, out int waterCount)) list.Add("hintJeneiMercury".lang(waterCount.ToString()));
        if (readySummon != null)
        {
            string summonName = readySummon + "_formalname";
            list.Add("hintJeneiSummonReady".lang(summonName.langGame()));
        }
    }
}