using System.Collections.Generic;
using System.Linq;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
using BardMod.Stats;
using UnityEngine;
namespace BardMod.Elements.Abilities.Niyon;

public class SpNebulosaFrojd : Spell
{

    public override int PerformDistance => 4;
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

    public override bool CanPerform()
    {
        if (CC == TC || TC == null || CC.Dist(TC) > PerformDistance)
        {
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        // TODO: CUSTOM EFFECT?
        float num = 0f;
        Card tC = TC;
        HashSet<int> hashSet = new HashSet<int>();
        foreach (Card item in _map.Cards.ToList())
        {
            if (!CC.IsAliveInCurrentZone)
            {
                break;
            }
            if (!item.IsAliveInCurrentZone ||
                item == CC ||
                item.isChara && item != tC && !item.Chara.IsHostile(CC) ||
                !item.isChara && !item.trait.CanBeAttacked ||
                item.Dist(CC) > PerformDistance ||
                !CC.CanSeeLos(item))
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
                ExecuteAttack();
            }
        }

        return true;
    }

    private bool ExecuteAttack()
    {
        // Deal Impact damage
        // Apply Charmed
        int damage = HelperFunctions.SafeDice(Constants.NebulosaFrojdName, GetPower(CC));
        // TC.DamageHP(dmg: damage, ele: Constants.EleImpact, eleP: 100, attackSource: AttackSource.None, origin: CC);
        BardCardPatches.CachedInvoker.Invoke(
            TC,
            new object[] { damage, Constants.EleImpact, 100, AttackSource.None, CC }
        );
        if (TC.isChara) TC.Chara.AddCondition<ConCharmed>(force: true);
        return true;
    }
}