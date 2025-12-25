using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.HolyKnight;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.HolyKnight;

public class ActHolyBanner : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHolyKnight) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HolyKnightId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActHolyBannerId)) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            type = CostType.None,
            cost = 0
        };
    }
    
    public override bool Perform()
    {
        int power = this.GetPower(CC);
        int levelOverride = power / 15;
        if (CC.IsPCFaction) levelOverride = Math.Max(player.stats.deepest, levelOverride);
        Chara bannerMob = CharaGen.Create(Constants.HolyBannerCharaId);
        bannerMob.SetSummon(10);
        bannerMob.SetLv(levelOverride);
        bannerMob.interest = 0;
        _zone.AddCard(bannerMob, TP);
        bannerMob.PlayEffect("teleport");
        bannerMob.MakeMinion(CC);
        // Flowers are not killable.
        bannerMob.AddCondition<RadiantAura>(power);
        return true;
    }
}