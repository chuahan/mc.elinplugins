using NierMod.Common;
using UnityEngine;
namespace NierMod.Elements;

internal class SpSummonDeath : Spell
{
    public override bool CanPerform()
    {
        if (CC.currentZone.FindChara(Constants.deathCharaId) != null ||
            CC.currentZone.CountMinions(CC) >= CC.MaxSummon)
        {
            return false;
        }
        return true;
    }

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override bool Perform()
    {
        if (CC.currentZone.FindChara(Constants.deathCharaId) != null ||
            CC.currentZone.CountMinions(CC) >= CC.MaxSummon)
        {
            CC.Say("summonDeathFail", CC);
            return false;
        }

        Point summonPoint = CC.pos.GetNearestPoint(allowChara: false);
        if (summonPoint == null || !summonPoint.IsValid)
        {
            CC.Say("summonDeathFail", CC);
            return false;
        }

        CC.Say("summonDeathSuccess", CC);
        //CC.PlaySound("death_screech");
        Chara deathSummon = CharaGen.Create(Constants.deathCharaId);
        deathSummon.c_summonDuration = 130;
        deathSummon.isSummon = true;
        deathSummon.SetLv(CC.LV * 13);
        deathSummon.interest = 0;
        CC.currentZone.AddCard(deathSummon, summonPoint);
        deathSummon.PlayEffect("teleport");
        deathSummon.MakeMinion(CC);
        return true;
    }
}