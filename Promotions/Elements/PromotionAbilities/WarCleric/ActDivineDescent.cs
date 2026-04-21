using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.WarCleric;
namespace PromotionMod.Elements.PromotionAbilities.WarCleric;

public class ActDivineDescent : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatWarCleric;
    public override string PromotionString => Constants.WarClericId;
    public override int Cooldown => 1440;
    public override int AbilityId => Constants.ActDivineDescentId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;

    public override bool CanPerformExtra()
    {
        return base.CanPerform();
    }

    public override int GetPower(Card c)
    {
        int basePower = base.GetPower(c);
        basePower += c.Evalue(SKILL.faith) * 4 + 30;
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
                    ActEffect.DamageEle(CC, EffectId.Ball, power, Element.Create(Constants.EleHoly, power / 10), new List<Point>
                    {
                        target.pos
                    }, new ActRef
                    {
                        act = this
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

        // 1 day cooldown.
        CC.AddCooldown(AbilityId, Cooldown);
        return true;
    }
}