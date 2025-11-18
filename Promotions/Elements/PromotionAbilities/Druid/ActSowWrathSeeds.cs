using System;
using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Druid;
namespace PromotionMod.Elements.PromotionAbilities.Druid;

/// <summary>
///     Druid Ability
///     Picks a point and creates a debuffing flower in the area.
///     Entangling Bloom - Binds enemies with Entangle.
///     Paralytic Bloom - Afflicts paralysis to enemies.
///     Toxic Bloom - Afflicts poison to enemies.
///     Soporific Bloom - Afflicts sleep to enemies.
/// </summary>
public class ActSowWrathSeeds : Ability
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

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        // Randomly pick one of the flowers to spawn.
        // Can only have one of each flower, except for Warrior Ents.
        List<string> spawnable = new List<string>
        {
            Constants.DruidWarriorEntCharaId
        };
        if (CC.currentZone.FindChara(Constants.DruidEntangleFlowerCharaId) == null) spawnable.Add(Constants.DruidEntangleFlowerCharaId);
        if (CC.currentZone.FindChara(Constants.DruidParalyticFlowerCharaId) == null) spawnable.Add(Constants.DruidParalyticFlowerCharaId);
        if (CC.currentZone.FindChara(Constants.DruidToxicFlowerCharaId) == null) spawnable.Add(Constants.DruidToxicFlowerCharaId);
        if (CC.currentZone.FindChara(Constants.DruidSoporificFlowerCharaId) == null) spawnable.Add(Constants.DruidSoporificFlowerCharaId);
        string toSummon = spawnable.RandomItem();

        // 1 in 10 chance for PC to summon Nature's Wrath instead.
        if (CC.IsPC && CC.currentZone.FindChara(Constants.DruidNaturesWrathCharaId) == null)
        {
            if (EClass.rnd(10) == 0)
            {
                toSummon = Constants.DruidNaturesWrathCharaId;
            }
        }

        int power = GetPower(CC);

        bool flowerSummoned = toSummon is Constants.DruidEntangleFlowerCharaId or Constants.DruidParalyticFlowerCharaId or Constants.DruidToxicFlowerCharaId
                or Constants.DruidSoporificFlowerCharaId;
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
            case Constants.DruidEntangleFlowerCharaId:
                plant.AddCondition<EntanglingAura>(power);
                break;
            case Constants.DruidParalyticFlowerCharaId:
                plant.AddCondition<ParalyzingAura>(power);
                break;
            case Constants.DruidToxicFlowerCharaId:
                plant.AddCondition<ToxicAura>(power);
                break;
            case Constants.DruidSoporificFlowerCharaId:
                plant.AddCondition<SleepAura>(power);
                break;
            case Constants.DruidNaturesWrathCharaId:
                plant.AddCondition<EntanglingAura>(power);
                plant.AddCondition<ToxicAura>(power);
                break;

        }
        return true;
    }
}