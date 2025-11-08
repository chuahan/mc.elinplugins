using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.WarCleric;
namespace PromotionMod.Elements.PromotionAbilities.WarCleric;

public class ActDivineDescent : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatWarCleric) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.WarClericId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActDivineDescentId)) return false;
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

    public override int GetPower(Card c)
    {
        int basePower = base.GetPower(c);
        basePower += c.Evalue(Constants.FaithId) * 4 + 30;
        return basePower;
    }

    public override bool Perform()
    {
        int power = GetPower(CC);
        int healPower = HelperFunctions.SafeDice(Constants.WarClericDivineDescentAlias, power);
        CC.AddCondition<ConDivineDescent>(GetPower(CC));

        // Cause a massive holy explosion that heals yourself and allies, damages enemies.
        List<Chara> targetsHit = new List<Chara>();
        Effect spellEffect = Effect.Get("Element/ball_Holy");
        foreach (Point tile in _map.ListPointsInCircle(CC.pos, 3f, false, false))
        {
            int distance = tile.Distance(CC.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target)))
            {
                // Damage Hostiles and apply Fear
                if (target.IsHostile(CC))
                {
                    ActEffect.DamageEle(CC, EffectId.Ball, power, Element.Create(Constants.EleHoly, power / 10), new List<Point>{target.pos}, new ActRef()
                    {
                        act = this,
                    });
                    target.AddCondition<ConFear>(power);
                }
                else
                {
                    // Heal allies.
                    target.HealHP(healPower);
                    // Try to purge debuffs.
                    foreach (Condition debuff in target.conditions.Copy())
                    {
                        if (debuff.Type != ConditionType.Debuff || debuff.IsKilled || EClass.rnd(power * 2) <= EClass.rnd(debuff.power)) continue;
                        CC.Say("removeHex", TC, debuff.Name.ToLower());
                        debuff.Kill();
                    }
                }

                // Mark Target as hit.
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion,
            float delay = distance * 0.7F;
            TweenUtil.Delay(delay, delegate
            {
                spellEffect.Play(tile, 0f, tile);
            });
        }
        CC.AddCooldown(Constants.ActDivineDescentId, 300);
        return true;
    }
}