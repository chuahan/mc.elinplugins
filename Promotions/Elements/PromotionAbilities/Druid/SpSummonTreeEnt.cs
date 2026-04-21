using System;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Druid;

public class SpSummonTreeEnt : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatDruid;
    public override string PromotionString => Constants.DruidId;
    public override int AbilityId => Constants.SpSummonTreeEntId;
    public override bool CanPerformExtra()
    {
        if (CC.currentZone.CountMinions(CC) >= CC.MaxSummon) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        for (int i = 0; i < 5; i++)
        {
            while (CC.currentZone.CountMinions(CC) < CC.MaxSummon)
            {
                Chara plant = CharaGen.Create(Constants.DruidWarriorEntCharaId);
                // Normal summon leveling.
                // For PCs Ent Warriors summons can scale to your deepest achieved depth instead.
                int power = GetPower(CC);
                int levelOverride = CC.LV * (100 + power / 10) / 100 + power / 30;
                if (CC.IsPCFaction) levelOverride = Math.Max(player.stats.deepest, levelOverride);
                plant.SetLv(levelOverride);
                plant.interest = 0;
                CC.currentZone.AddCard(plant, TP);
                plant.PlayEffect("mutation");
                plant.MakeMinion(CC);
            }
        }

        return true;
    }
}