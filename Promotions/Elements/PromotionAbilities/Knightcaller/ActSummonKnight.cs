using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Knightcaller;
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

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
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
        // BALANCE: Do we want to allow multiple captains active at once?
        if (CC.currentZone.ListMinions(CC).FirstOrDefault(x => x.trait is TraitSpiritKnightCaptain) != null)
        {
            // There can only be one of each active knight captain in the zone.
            List<string> knightCaptains = new List<string>();
            if (CC.currentZone.FindChara(Constants.ValeroCharaId) == null) knightCaptains.Add(Constants.ValeroCharaId);
            if (CC.currentZone.FindChara(Constants.DinatogCharaId) == null) knightCaptains.Add(Constants.DinatogCharaId);
            if (CC.currentZone.FindChara(Constants.ArkunCharaId) == null) knightCaptains.Add(Constants.ArkunCharaId);
            if (CC.currentZone.FindChara(Constants.AlestieCharaId) == null) knightCaptains.Add(Constants.AlestieCharaId);
            if (CC.currentZone.FindChara(Constants.EctoleCharaId) == null) knightCaptains.Add(Constants.EctoleCharaId);
            if (CC.currentZone.FindChara(Constants.RolingerCharaId) == null) knightCaptains.Add(Constants.RolingerCharaId);
            if (CC.currentZone.FindChara(Constants.RolingerCharaId) == null) knightCaptains.Add(Constants.DiasCharaId);
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
        EquipKnight(knight);

        // Add Summoning Sickness to the caster.
        int sicknessStacks = 1;
        ConSummoningSickness sickness = CC.GetCondition<ConSummoningSickness>();
        if (sickness != null)
        {
            sicknessStacks += sickness.power;
            sickness.Kill();
        }
        CC.AddCondition<ConSummoningSickness>(sicknessStacks, true);

        return true;
    }

    public void EquipKnight(Chara chara)
    {
        switch (chara.id)
        {
            case Constants.KnightArcherCharaId:
            case Constants.DinatogCharaId:
                chara.AddThing(ThingGen.Create("dagger", -1, chara.LV));
                chara.AddThing(ThingGen.Create("bow", -1, chara.LV));

                chara.AddThing(ThingGen.Create("hat_feather", -1, chara.LV));
                chara.AddThing(ThingGen.Create("armor_light", -1, chara.LV));
                chara.AddThing(ThingGen.Create("gloves_light", -1, chara.LV));
                chara.AddThing(ThingGen.Create("girdle_corset", -1, chara.LV));
                chara.AddThing(ThingGen.Create("cloak_foreign", -1, chara.LV));
                chara.AddThing(ThingGen.Create("boots_tight", -1, chara.LV));
                break;

            case Constants.KnightHermitCharaId:
            case Constants.AlestieCharaId:
                chara.AddThing(ThingGen.Create("dagger", -1, chara.LV));
                chara.AddThing(ThingGen.Create("dagger", -1, chara.LV));

                chara.AddThing(ThingGen.Create("helm", -1, chara.LV));
                chara.AddThing(ThingGen.Create("armor_light", -1, chara.LV));
                chara.AddThing(ThingGen.Create("gloves_light", -1, chara.LV));
                chara.AddThing(ThingGen.Create("girdle_corset", -1, chara.LV));
                chara.AddThing(ThingGen.Create("cloak_foreign", -1, chara.LV));
                chara.AddThing(ThingGen.Create("boots_tight", -1, chara.LV));
                break;

            case Constants.KnightLancerCharaId:
            case Constants.ValeroCharaId:
                chara.AddThing(ThingGen.Create("spear", -1, chara.LV));
                chara.AddThing(ThingGen.Create("shield_knight", -1, chara.LV));

                chara.AddThing(ThingGen.Create("helm_knight", -1, chara.LV));
                chara.AddThing(ThingGen.Create("armor_breast", -1, chara.LV));
                chara.AddThing(ThingGen.Create("gloves_plate", -1, chara.LV));
                chara.AddThing(ThingGen.Create("girdle_plate", -1, chara.LV));
                chara.AddThing(ThingGen.Create("cloak_armored", -1, chara.LV));
                chara.AddThing(ThingGen.Create("boots_heavy", -1, chara.LV));
                break;

            case Constants.KnightPriestessCharaId:
            case Constants.EctoleCharaId:
                chara.AddThing(ThingGen.Create("blunt_mace", 40, chara.LV));

                chara.AddThing(ThingGen.Create("hat_wizard", -1, chara.LV));
                chara.AddThing(ThingGen.Create("robe_pope", -1, chara.LV));
                chara.AddThing(ThingGen.Create("gloves", -1, chara.LV));
                chara.AddThing(ThingGen.Create("girdle_corset", -1, chara.LV));
                chara.AddThing(ThingGen.Create("cloak_light", -1, chara.LV));
                chara.AddThing(ThingGen.Create("boots_", -1, chara.LV));
                break;

            case Constants.KnightDuelistCharaId:
            case Constants.ArkunCharaId:
                chara.AddThing(ThingGen.Create("sword", -1, chara.LV));

                chara.AddThing(ThingGen.Create("helm_knight", -1, chara.LV));
                chara.AddThing(ThingGen.Create("armor_light", -1, chara.LV));
                chara.AddThing(ThingGen.Create("gloves_thick", -1, chara.LV));
                chara.AddThing(ThingGen.Create("girdle_corset", -1, chara.LV));
                chara.AddThing(ThingGen.Create("cloak_foreign", -1, chara.LV));
                chara.AddThing(ThingGen.Create("boots_heavy", -1, chara.LV));

                break;

            case Constants.KnightWarriorCharaId:
            case Constants.DiasCharaId:
                chara.AddThing(ThingGen.Create("axe_battle", -1, chara.LV));

                chara.AddThing(ThingGen.Create("helm_knight", -1, chara.LV));
                chara.AddThing(ThingGen.Create("armor_breast", -1, chara.LV));
                chara.AddThing(ThingGen.Create("gloves_plate", -1, chara.LV));
                chara.AddThing(ThingGen.Create("girdle_plate", -1, chara.LV));
                chara.AddThing(ThingGen.Create("cloak_armored", -1, chara.LV));
                chara.AddThing(ThingGen.Create("boots_heavy", -1, chara.LV));
                break;

            case Constants.KnightWizardCharaId:
            case Constants.RolingerCharaId:
                chara.AddThing(ThingGen.Create("staff", -1, chara.LV));

                chara.AddThing(ThingGen.Create("hat_wizard", -1, chara.LV));
                chara.AddThing(ThingGen.Create("robe_pope", -1, chara.LV));
                chara.AddThing(ThingGen.Create("gloves", -1, chara.LV));
                chara.AddThing(ThingGen.Create("girdle_corset", -1, chara.LV));
                chara.AddThing(ThingGen.Create("cloak_light", -1, chara.LV));
                chara.AddThing(ThingGen.Create("boots_", -1, chara.LV));
                break;
        }
    }
}