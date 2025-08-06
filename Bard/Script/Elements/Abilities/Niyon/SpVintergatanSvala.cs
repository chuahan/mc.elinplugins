using System;
using System.Collections.Generic;
using System.Linq;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
using BardMod.Stats.BardSongConditions;
using UnityEngine;
namespace BardMod.Elements.Abilities.Niyon;

public class SpVintergatanSvala : Spell
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override Cost GetCost(Chara c)
    {
        Cost result2 = default(Cost);
        result2.type = CostType.MP;

        int num = EClass.curve(Value, 50, 10);
        result2.cost = source.cost[0] * (100 + (!source.tag.Contains("noCostInc") ? num * 3 : 0)) / 100;

        // Higher Music skill will reduce mana costs.
        if (c != null)
        {
            int musicSkill = c.Chara.Evalue(Constants.MusicSkill);
            result2.cost *= 100 / (100 + musicSkill);
        }

        if ((c == null || !c.IsPC) && result2.cost > 2)
        {
            result2.cost /= 2;
        }

        return result2;
    }

    public override int GetPower(Card bard)
    {
        return HelperFunctions.GetBardPower(base.GetPower(bard), bard);
    }

    public override bool Perform()
    {
        // Play Effect
        // TODO: CUSTOM EFFECT?
        //Effect spellEffect = Effect.Get("Element/ball_Cold");
        //spellEffect.Play(CC.pos);
        //CC.PlaySound("spell_ball");
        int damage = HelperFunctions.SafeDice(Constants.VintergatanSvalaName, GetPower(CC));
        // TC.DamageHP(dmg: damage, ele: Constants.EleCold, eleP: 100, attackSource: AttackSource.MagicSword, origin: CC);
        BardCardPatches.CachedInvoker.Invoke(
            TC,
            new object[] { damage, Constants.EleCold, 100, AttackSource.MagicSword, CC }
        );
        TC.PlaySound("ab_magicsword");
        TC.PlayEffect("hit_slash").SetScale(1f);

        // All allies nearby refresh bard songs.
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 4f, CC, true, false);
        foreach (Chara target in targets)
        {
            List<ConBardSong> bardConditions = target.conditions.OfType<ConBardSong>().Where(c => c.SongType != Constants.BardSongType.Finale).ToList();
            foreach (ConBardSong condition in bardConditions)
            {
                condition.RefreshBardSong();
            }
            target.PlayEffect("buff");
        }

        // Summon a Star Hand ally if there already does not exist one on the battlefield.
        if (CC.currentZone.FindChara(Constants.StarHandCharaId) == null && CC.currentZone.CountMinions(CC) <= CC.MaxSummon)
        {
            Point summonPoint = CC.pos.GetNearestPoint(false, false);
            Chara summon = CharaGen.Create(Constants.StarHandCharaId);
            summon.c_summonDuration = 10;
            summon.isSummon = true;
            summon.SetLv(Math.Max(CC.LV, GetPower(CC) / 100));
            summon.interest = 0;
            summon.isBerserk = true;
            summon.Chara.PlayEffect("teleport");
            summon.Chara.MakeMinion(CC);
            CC.currentZone.AddCard(summon, summonPoint);
        }

        return true;
    }
}