using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Trait;
namespace PromotionMod.Elements.PromotionAbilities.Knightcaller;

public class ActSummonKnight : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatKnightcaller) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.KnightcallerId.lang()));
            return false;
        }
        if (CC.currentZone.CountMinions(CC) >= CC.MaxSummon) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        // Randomly pick one of the knights to spawn.
        // If you have no active captain, 1/4 chance of summoning a captain first.
        // Can only have one captain.
        List<string> knights = new List<string>
        {
            Constants.KnightArcherCharaId,
            Constants.KnightHermitCharaId,
            Constants.KnightLancerCharaId,
            Constants.KnightPriestessCharaId,
            Constants.KnightDuelistCharaId,
            Constants.KnightWarriorCharaId,
            Constants.KnightWizardCharaId
        };
        string toSummon = knights.RandomItem();
        bool captainSummoned = false;
        // If you have no active knight captain, 1/4 chance to summon one instead.
        if (CC.currentZone.ListMinions(CC).FirstOrDefault(x => x.trait is TraitSpiritKnightCaptain) != null)
        {
            // There can only be one of each active knight captain in the zone.
            List<string> knightCaptains = new List<string>
            {
            };
            if (CC.currentZone.FindChara(Constants.ValeroCharaId) == null) knightCaptains.Add(Constants.ValeroCharaId);
            if (CC.currentZone.FindChara(Constants.DinatogCharaId) == null) knightCaptains.Add(Constants.DinatogCharaId);
            if (CC.currentZone.FindChara(Constants.ArkunCharaId) == null) knightCaptains.Add(Constants.ArkunCharaId);
            if (CC.currentZone.FindChara(Constants.AlestieCharaId) == null) knightCaptains.Add(Constants.AlestieCharaId);
            if (CC.currentZone.FindChara(Constants.EctoleCharaId) == null) knightCaptains.Add(Constants.EctoleCharaId);
            if (CC.currentZone.FindChara(Constants.RolingerCharaId) == null) knightCaptains.Add(Constants.RolingerCharaId);
            if (knightCaptains.Count > 0 && EClass.rnd(4) == 0)
            {
                captainSummoned = true;
                toSummon = knightCaptains.RandomItem();
            }
        }

        // Normal summon leveling.
        // For PCs summons can scale to your deepest achieved depth instead.
        // Captains come at 10% higher level.
        Chara knight = CharaGen.Create(toSummon);
        knight.isSummon = true;
        int power = GetPower(CC);
        int levelOverride = CC.LV * (100 + power / 10) / 100 + power / 30;
        if (CC.IsPC) levelOverride = Math.Max(player.stats.deepest, levelOverride);
        if (captainSummoned) levelOverride = HelperFunctions.SafeMultiplier(levelOverride, 1.1F);
        knight.SetLv(levelOverride);
        knight.interest = 0;
        CC.currentZone.AddCard(knight, TP);
        knight.PlayEffect("curse");
        knight.MakeMinion(CC);
        return true;
    }
}