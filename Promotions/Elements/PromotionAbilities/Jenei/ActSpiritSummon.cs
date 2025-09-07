using System;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats.Jenei;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActSpiritSummon : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJenei) == 0) return false;
        if (CC.HasCooldown(Constants.ActSpiritSummonId)) return false;

        // NPCs can summon a random summon every 30 turns.
        if (CC.IsPC && !CC.HasCondition<ConJenei>()) return false;
        if (CC.currentZone.CountMinions(CC) >= CC.MaxSummon) return false;
        return base.CanPerform();
    }

    // Spirit Summon Costs nothing.
    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 0,
            type = CostType.None
        };
    }

    public override bool Perform()
    {
        // NPCs can summon a random summon with higher cooldown.
        ConJenei djinnStockpile = CC.GetCondition<ConJenei>();
        string? summon = FeatJenei.JeneiSummons.GetSummon(djinnStockpile.GetElementalStockpile());
        if (!CC.IsPC) summon = FeatJenei.JeneiSummons.AllSummons.Select(x => x.SummonId).ToList().RandomItem();

        // If the zone already has this summon active, fizzle.
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
        if (CC.IsPC)
        {
            djinnStockpile.EmptyStockpile();
            CC.AddCooldown(Constants.ActSpiritSummonId, 1);
        }
        else
        {
            CC.AddCooldown(Constants.ActSpiritSummonId, 30);
        }

        return true;
    }
}