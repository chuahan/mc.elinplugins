using System;
using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities;

public class SpSummonSkeleton : Spell
{
    private static List<string> SkeletonOptions = new List<string>
    {
        Constants.NecromancerSkeletonWarriorCharaId,
        Constants.NecromancerSkeletonMageCharaId
    };

    public static readonly List<int> MageElements = new List<int>{
        Constants.EleFire,
        Constants.EleCold,
        Constants.EleLightning,
        Constants.ElePoison,
    };

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatNecromancer) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.NecromancerId.lang()));
            return false;
        }
        if (CC.currentZone.CountMinions(CC) >= CC.MaxSummon) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        SpSummonSkeleton.SummonSkeleton(CC, TP, GetPower(CC));
        return true;
    }

    public static void SummonSkeleton(Chara caster, Point target, int power, int deathknightChance = 20, int targetLevel = 0)
    {
        // Can't go over cap of minions.
        if (CC.currentZone.CountMinions(CC) >= CC.MaxSummon)
        {
            caster.Say("necromancer_limit".langGame());
            return;
        }

        string summonName = SkeletonOptions.RandomItem();
        if (EClass.rnd(deathknightChance) == 0) summonName = Constants.NecromancerDeathKnightCharaId;

        Chara summon = CharaGen.Create(summonName);
        // Normal summon leveling.
        // For PCs Summons can scale to your deepest achieved depth instead.
        int levelOverride = Math.Max(caster.LV, targetLevel) * (100 + power / 10) / 100 + power / 30;
        if (caster.IsPC) levelOverride = Math.Max(player.stats.deepest, levelOverride);
        summon.SetLv(levelOverride);
        summon.interest = 0;
        caster.currentZone.AddCard(summon, target);
        summon.PlayEffect("mutation");
        summon.MakeMinion(caster);
        //summon.c_summonDuration

        // Equip the Skeleton.
        switch (summonName)
        {
            case Constants.NecromancerSkeletonMageCharaId:
                // Skeleton Mages will have a random element between Fire, Cold, Lightning, and Poison
                summon.SetMainElement(Constants.ElementAliasLookup[MageElements.RandomItem()], 50, elemental: true);
                summon.AddThing(ThingGen.Create("staff", 40, summon.LV));
                break;
            case Constants.NecromancerSkeletonWarriorCharaId:
                summon.AddThing(ThingGen.Create("sword", 40, summon.LV));
                summon.AddThing(ThingGen.Create("shield_knight", 40, summon.LV));
                break;
            case Constants.NecromancerDeathKnightCharaId:
                summon.AddThing(ThingGen.Create("sword", 40, summon.LV));
                summon.AddThing(ThingGen.Create("shield_knight", 40, summon.LV));
                summon.AddThing(ThingGen.Create("helm_knight", 40, summon.LV));
                summon.AddThing(ThingGen.Create("armor_breast", 40, summon.LV));
                summon.AddThing(ThingGen.Create("boots_heavy", 40, summon.LV));
                break;
        }
    }
}