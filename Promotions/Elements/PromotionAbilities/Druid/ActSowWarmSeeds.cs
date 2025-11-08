using System;
using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Druid;
namespace PromotionMod.Elements.PromotionAbilities.Druid;

/// <summary>
///     Druid Ability
///     Picks a point and creates a buffing flowers in the area.
///     Soothing Bloom - Heals over time.
///     Warding Bloom - Allies gain protection.
///     Serene Bloom - Removes Debuffs
/// </summary>
public class ActSowWarmSeeds : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatDruid) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.DruidId.lang()));
            return false;
        }
        if (CC.currentZone.CountMinions(CC) >= CC.MaxSummon) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        // Randomly pick one of the flowers to spawn.
        // Can only have one of each flower, except for Warrior Ents.
        List<string> spawnable = new List<string>
        {
            Constants.DruidWarriorEntCharaId
        };
        if (CC.currentZone.FindChara(Constants.DruidSoothingBloomCharaId) == null) spawnable.Add(Constants.DruidSoothingBloomCharaId);
        if (CC.currentZone.FindChara(Constants.DruidWardingBloomCharaId) == null) spawnable.Add(Constants.DruidWardingBloomCharaId);
        if (CC.currentZone.FindChara(Constants.DruidSereneBloomCharaId) == null) spawnable.Add(Constants.DruidSereneBloomCharaId);
        string toSummon = spawnable.RandomItem();

        // 1 in 10 chance for PC to summon Nature's Warmth instead.
        if (CC.IsPC && CC.currentZone.FindChara(Constants.DruidNaturesWarmthCharaId) == null)
        {
            if (EClass.rnd(10) == 0)
            {
                toSummon = Constants.DruidNaturesWarmthCharaId;
            }
        }
        
        int power = GetPower(CC);
        bool flowerSummoned = toSummon is Constants.DruidSoothingBloomCharaId or Constants.DruidWardingBloomCharaId or Constants.DruidSereneBloomCharaId;
        Chara plant = CharaGen.Create(toSummon);
        plant.isSummon = true;
        if (flowerSummoned)
        {
            // Flowers only last 30 turns
            plant.c_summonDuration = 30;
            plant.SetLv(1);
        }
        else
        {
            // Normal summon leveling.
            // For PCs Ent Warriors and Nature's Warmth summons can scale to your deepest achieved depth instead.
            int levelOverride = CC.LV * (100 + power / 10) / 100 + power / 30;
            if (CC.IsPC) levelOverride = Math.Max(player.stats.deepest, levelOverride);
            plant.SetLv(levelOverride);
        }

        plant.interest = 0;
        CC.currentZone.AddCard(plant, TP);
        plant.PlayEffect("mutation");
        plant.MakeMinion(CC);

        // Flowers are not killable.
        if (flowerSummoned) plant.AddCondition<ConInvulnerable>(30000);
        
        // Apply the Aura Buff
        switch (toSummon)
        {
            case Constants.DruidSoothingBloomCharaId:
                plant.AddCondition<HealingAura>(power);
                break;
            case Constants.DruidWardingBloomCharaId:
                plant.AddCondition<ProtectionAura>(power);
                break;
            case Constants.DruidSereneBloomCharaId:
                plant.AddCondition<EsunaAura>(power);
                break;
            case Constants.DruidNaturesWarmthCharaId:
                plant.AddCondition<HealingAura>(power);
                plant.AddCondition<EsunaAura>(power);
                break;
        }

        return true;
    }
}