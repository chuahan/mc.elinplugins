using System.Collections.Generic;
using System.Linq;
using BardMod.Common;
using BardMod.Stats;

namespace BardMod.Elements.Abilities.Selena;

public class ActThunderousTransposition : BardAbility
{
    public override int PerformDistance => 3;

    public override bool CanPerform()
    {
        if (Act.CC == Act.TC || Act.TC == null || Act.CC.Dist(Act.TC) > PerformDistance)
        {
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        // Modified Effect if Selena has enough Rhythm - Inflicts Freeze on all targets.
        bool playerRhythm = CC.Evalue(Constants.FeatTimelessSong) > 0 && CC.IsPCParty && EClass.pc.HasCondition<ConRhythm>();
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
        
        float num = 0f;
        Card tC = Act.TC;
        HashSet<int> hashSet = new HashSet<int>();
        foreach (Card item in EClass._map.Cards.ToList())
        {
            if (!Act.CC.IsAliveInCurrentZone)
            {
                break;
            }
            if (!item.IsAliveInCurrentZone ||
                item == Act.CC ||
                (item.isChara && item != tC && !item.Chara.IsHostile(Act.CC)) ||
                (!item.isChara && !item.trait.CanBeAttacked) ||
                item.Dist(Act.CC) > PerformDistance ||
                !Act.CC.CanSeeLos(item))
            {
                continue;
            }
            Point pos = item.pos;
            if (!hashSet.Contains(pos.index))
            {
                hashSet.Add(pos.index);
                TweenUtil.Delay(num, delegate
                {
                    pos.PlayEffect("ab_swarm");
                    pos.PlaySound("ab_swarm");
                });
                if (num < 1f)
                {
                    num += 0.07f;
                }
                new ActThunderousTranspositionMelee(){SpecialActive = specialEffect}.Perform(Act.CC, item);
            }
        }
        
        // If it's not the PC, add Rhythm.
        if (!CC.IsPC)
        {
            rhythm ??= CC.AddCondition<ConRhythm>() as ConRhythm;
            rhythm?.ModStacks(3);   
        };
        
        return true;
    }
}