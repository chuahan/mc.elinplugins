using System;
using System.Net.Configuration;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActSpiritSummon : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJenei) == 0) return false;
        if (CC.HasCooldown(Constants.ActSpiritSummonId)) return false;
        if (!CC.HasCondition<ConJenei>()) return false;
        if (CC.currentZone.CountMinions(CC) >= CC.MaxSummon) return false;
        return base.CanPerform();
    }
    
    // Spirit Summon Costs nothing.
    public override Cost GetCost(Chara c)
    {
        return new Cost()
        {
            cost = 0,
            type = CostType.None,
        };
    }
    
    public override bool Perform()
    {
        ConJenei djinnStockpile = CC.GetCondition<ConJenei>();
        string? summon = FeatJenei.JeneiSummons.GetSummon(djinnStockpile.ElementalStockpile);
        // If the zone alraedy has this summon active, fizzle.
        if (CC.currentZone.FindChara(summon) != null) return false;
        if (summon == null) return false;
        
        // Summon - For PCs summons can scale to your deepest achieved depth instead.
        Chara jeneiSummon = CharaGen.Create(summon);
        jeneiSummon.isSummon = true;
        int power = GetPower(CC);
        int levelOverride = CC.LV * (100 + power / 10) / 100 + power / 30;
        if (CC.IsPC) levelOverride = Math.Max(player.stats.deepest, levelOverride);
        jeneiSummon.SetLv(levelOverride);
        jeneiSummon.interest = 0;
        CC.currentZone.AddCard(jeneiSummon, TP);
        jeneiSummon.PlayEffect("aura_heaven");
        jeneiSummon.MakeMinion(CC);
        
        // Get Text
        string summonName = summon + "_formalname";
        CC.TalkRaw("jenei_summonphrase".langGame(summonName.langGame()));
        Msg.Say("jenei_summon".langGame(CC.NameSimple, summonName.langGame()));
        
        // Empty stockpile.
        djinnStockpile.EmptyStockpile();
        CC.AddCooldown(Constants.ActSpiritSummonId, 1);
        return true;
    }
}