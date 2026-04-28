using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.DreadKnight;
namespace PromotionMod.Elements.PromotionAbilities.DreadKnight;

public class ActDarkBurst : PromotionSpellAbility
{

    private float _effectRadius = 3F;
    public override int PromotionId => Constants.FeatDreadKnight;
    public override string PromotionString => Constants.DreadKnightId;
    public override int AbilityId => Constants.ActDarkBurstId;

    public override bool CanPerformExtra(bool verbose)
    {
        if (GetHPCost(CC) > CC.hp)
        {
            if (CC.IsPC && verbose) Msg.Say("hpcostability_notenoughhp".langGame());
            return false;
        }

        return true;
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

    public override void OnMarkMapHighlights()
    {
        float adjustedEffectRadius = _effectRadius;
        ConDarkTraces darkTrace = CC.GetCondition<ConDarkTraces>();
        if (darkTrace != null)
        {
            adjustedEffectRadius += darkTrace.GetStacks();
        }
        List<Point> list = _map.ListPointsInCircle(CC.pos, adjustedEffectRadius);
        if (list.Count == 0)
        {
            list.Add(CC.pos.Copy());
        }
        foreach (Point item in list)
        {
            item.SetHighlight(8);
        }
    }

    public override bool Perform()
    {
        ConDarkTraces darkTrace = CC.GetCondition<ConDarkTraces>();
        float hpCost = 0.1F;
        if (darkTrace != null)
        {
            // Dark Traces increases the HP Cost by 10% each stack
            hpCost += darkTrace.GetStacks() * .1F;
        }
        else
        {
            darkTrace = (ConDarkTraces)CC.AddCondition<ConDarkTraces>();
        }

        // User's consumed HP * 2 will be added as spellpower.
        int cost = (int)(CC.MaxHP * hpCost);
        CC.hp -= cost;

        int power = HelperFunctions.SafeAdd(GetPower(CC), HelperFunctions.SafeMultiplier(cost, 2));
        float adjustedEffectRadius = _effectRadius;
        adjustedEffectRadius += darkTrace.GetStacks();

        Effect spellEffect = Effect.Get("Element/ball_Nether");
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in _map.ListPointsInCircle(CC.pos, adjustedEffectRadius, false, false))
        {
            int distance = tile.Distance(CC.pos);

            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target) && target.IsHostile(CC)))
            {
                ActEffect.DamageEle(CC, EffectId.Ball, power, Element.Create(Constants.EleNether, power / 10), new List<Point>
                {
                    target.pos
                }, new ActRef
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