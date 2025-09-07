using System;
using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Machinist;

public class SpSummonTurret : Spell
{
    public static List<string> TurretOptions = new List<string>
    {
        Constants.MachinistRifleTurretCharaId,
        Constants.MachinistRailgunTurretCharaId,
        Constants.MachinistRocketTurretCharaId
    };

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatMachinist) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.MachinistId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        // Normal summon leveling.
        // For PCs summons can scale to your deepest achieved depth instead.
        string summonedGunType = TurretOptions.RandomItem();
        Chara summon = CharaGen.Create(summonedGunType);
        summon.isSummon = true;
        int power = GetPower(CC);
        int levelOverride = CC.LV * (100 + power / 10) / 100 + power / 30;
        if (CC.IsPC) levelOverride = Math.Max(player.stats.deepest, levelOverride);
        summon.SetLv(levelOverride);
        summon.interest = 0;
        CC.currentZone.AddCard(summon, TP);
        summon.PlayEffect("curse");
        summon.MakeMinion(CC);

        // Grant the summon a weapon based on the type of summon
        switch (summonedGunType)
        {
            case Constants.MachinistRailgunTurretCharaId:
                summon.AddThing(ThingGen.Create("gun_rail", lv: summon.LV));
                break;
            case Constants.MachinistRocketTurretCharaId:
                summon.AddThing(ThingGen.Create("gun_rocket", lv: summon.LV));
                break;
            default: //Constants.MachinistRifleTurretCharaId
                summon.AddThing(ThingGen.Create("gun_assault", lv: summon.LV));
                break;
        }
        return true;
    }
}