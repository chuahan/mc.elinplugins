using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.DreadKnight;

public class ActDarkAura : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatDreadKnight) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.DreadKnightId.lang()));
            return false;
        }

        if (GetHPCost(CC) > CC.hp)
        {
            if (CC.IsPC) Msg.Say("dreadknight_notenoughhp".lang());
            return false;
        }

        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 0,
            type = CostType.None
        };
    }
    
    // Apply Spell Enhance to this ability.
    public override int GetPower(Card c)
    {
        int power = base.GetPower(c);
        return power * Mathf.Max(100 + c.Evalue(411) - c.Evalue(93), 1) / 100;
    }

    public int GetHPCost(Chara c)
    {
        ConDarkTraces darkTrace = c.GetCondition<ConDarkTraces>();
        float hpCost = 0.1F;
        if (darkTrace != null)
        {
            // Dark Traces increases the HP Cost by 10% each stack
            hpCost += darkTrace.GetStacks() * .1F;
        }

        return (int)(c.MaxHP * hpCost);
    }
    
    public override bool Perform()
    {
        ConDarkTraces darkTrace = c.GetCondition<ConDarkTraces>();
        float hpCost = 0.1F;
        if (darkTrace != null)
        {
            // Dark Traces increases the HP Cost by 10% each stack
            hpCost += darkTrace.GetStacks() * .1F;
        }
        else
        {
            darkTrace = CC.AddCondition<ConDarkTraces>();
        }

        // User's consumed HP * 2 will be added as spellpower.
        int cost = (int)(CC.MaxHP * hpCost);
        CC.DamageHP(cost, AttackSource.Condition);

        int power = HelperFunctions.SafeAdd(this.GetPower(CC), HelperFunctions.SafeMultiplier(cost, 2));
        float damageRadius = 3F;
        damageRadius += darkTrace.GetStacks();
        
        Effect spellEffect = Effect.Get("Element/ball_Nether");
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in _map.ListPointsInCircle(CC.pos, damageRadius, false, false))
        {
            int distance = tile.Distance(CC.pos);

            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target) && target.IsHostile(CC)))
            {
                ActEffect.DamageEle(CC, EffectId.Ball, power, Element.Create(Constants.EleNether, power / 10), new List<Point>{target.pos}, new ActRef
                {
                    act = this
                });

                // Mark Target as hit.
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the effect.
            float delay = distance * 0.7F;
            TweenUtil.Delay(delay, delegate
            {
                spellEffect.Play(tile, 0f, tile);
            });
        }
        
        darkTrace.AddStacks(1);
        return true;
    }
}