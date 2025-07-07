using System.Collections.Generic;
using System.Linq;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Stats;
namespace BardMod.Elements.Abilities.Niyon;

public class ActNebulosaFrojd : BardAbility
{
    public override int PerformDistance => 4;

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
        // TODO: CUSTOM EFFECT?
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
                    pos.PlayEffect("wave_hit");
                    pos.PlaySound("Footstep/water");
                });
                if (num < 1f)
                {
                    num += 0.07f;
                }
                new ActNebulosaFrojdMelee(){}.Perform(Act.CC, item);
            }
        }
        
        return true;
    }
}