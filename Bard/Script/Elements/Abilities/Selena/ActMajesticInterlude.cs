using System;
using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Stats;
using BardMod.Stats.BardSongConditions;
using UnityEngine;
namespace BardMod.Elements.Abilities.Selena;

public class ActMajesticInterlude : Ability
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
        int musicSkill = c.Chara.Evalue(Constants.MusicSkill);
        result2.cost *= 100 / (100 + musicSkill);

        if (!c.IsPC && result2.cost > 2)
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
        // Modified Effect if Selena has enough Rhythm - All enemies are afflicted with ConEphemeralFlowersSong
        bool playerRhythm = CC.Evalue(Constants.FeatTimelessSong) > 0 && CC.IsPCParty && pc.HasCondition<ConRhythm>();
        ConRhythm? rhythm = CC.GetCondition<ConRhythm>();
        bool specialEffect = false;
        if (playerRhythm)
        {
            specialEffect = true;
        }
        else if (rhythm != null)
        {
            if (rhythm.GetStacks() >= 10)
            {
                rhythm.ModStacks(-10);
                specialEffect = true;
            }
        }

        // Summon 5 Phantom Flutists
        int phantomCount = 5;
        int phantomPower = 10;
        int phantomLevelCalculated = HelperFunctions.SafeMultiplier(CC.LV, phantomPower);
        for (int i = 0; i < phantomCount; i++)
        {
            Point summonPoint = CC.pos.GetNearestPoint(false, false);
            Chara phantom = CharaGen.Create(Constants.PhantomFlutistCharaId);
            phantom.c_summonDuration = 10;
            phantom.isSummon = true;
            phantom.SetLv(Math.Max(GetPower(CC) / 100, phantomLevelCalculated));
            phantom.interest = 0;
            CC.currentZone.AddCard(phantom, summonPoint);
            phantom.PlayEffect("teleport");
            phantom.MakeMinion(CC);

            ConInvulnerable invulnerable = new ConInvulnerable
            {
                value = 300
            };
            phantom.AddCondition(invulnerable);
        }

        if (specialEffect)
        {
            List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 4f, CC, false, false);
            foreach (Chara target in targets)
            {
                ConEphemeralFlowersSong bardDebuff = ConBardSong.Create(nameof(ConEphemeralFlowersSong), GetPower(CC), 30, true, CC) as ConEphemeralFlowersSong;
                TC.Chara.AddCondition(bardDebuff);
            }
        }

        // If it's not the PC, add Rhythm.
        if (!CC.IsPC)
        {
            rhythm ??= CC.AddCondition<ConRhythm>() as ConRhythm;
            rhythm?.ModStacks(3);
        }

        return true;
    }
}