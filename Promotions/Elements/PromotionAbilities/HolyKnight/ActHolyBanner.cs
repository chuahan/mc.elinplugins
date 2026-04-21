using System;
using PromotionMod.Common;
using PromotionMod.Stats.HolyKnight;
namespace PromotionMod.Elements.PromotionAbilities.HolyKnight;

public class ActHolyBanner : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatHolyKnight;
    public override string PromotionString => Constants.HolyKnightId;
    public override int Cooldown => 0;
    public override int AbilityId => Constants.ActHolyBannerId;

    public override bool CanPerformExtra()
    {
        // Cannot use it if they already placed a Holy Banner.
        if (CC.HasMinion(Constants.HolyBannerCharaId)) return false;
        // Cannot use if TP is not placeable.
        if (!TP.IsValid || !Los.IsVisible(CC.pos, TP)) return false;
        return true;
    }

    public override bool Perform()
    {
        int power = GetPower(CC);
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
        // TODO: Give Banner Metal 999?
        // The Banner should be Immortal, but will only last for 10 turns.
        return true;
    }
}