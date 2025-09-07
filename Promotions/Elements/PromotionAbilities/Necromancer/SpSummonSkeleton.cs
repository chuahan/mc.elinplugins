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
        string summonName = SkeletonOptions.RandomItem();
        Chara summon = CharaGen.Create(summonName);
        summon.isSummon = true;
        // Normal summon leveling.
        // For PCs Summons can scale to your deepest achieved depth instead.
        int power = GetPower(CC);
        int levelOverride = CC.LV * (100 + power / 10) / 100 + power / 30;
        if (CC.IsPC) levelOverride = Math.Max(player.stats.deepest, levelOverride);
        summon.SetLv(levelOverride);
        summon.interest = 0;
        CC.currentZone.AddCard(summon, TP);
        summon.PlayEffect("mutation");
        summon.MakeMinion(CC);

        // Equip the Skeleton.
        switch (summonName)
        {
            case Constants.NecromancerSkeletonMageCharaId:
                summon.AddThing(ThingGen.Create("staff", 40, summon.LV));
                break;
            default:
                summon.AddThing(ThingGen.Create("sword", 40, summon.LV));
                summon.AddThing(ThingGen.Create("shield_knight", 40, summon.LV));
                break;
        }

        return true;
    }
}