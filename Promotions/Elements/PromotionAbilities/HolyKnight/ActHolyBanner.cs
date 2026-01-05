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
        // Cannot use it if they already placed a Holy Banner.
        if (CC.HasMinion(Constants.HolyBannerCharaId)) return false;
        // Cannot use if TP is not placeable.
        if (!TP.IsValid || !Los.IsVisible(CC.pos, TP)) return false;
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
        // Add the Aura.
        bannerMob.AddCondition<RadiantAura>(power);
        return true;
    }
}