using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities.Gambler;
using PromotionMod.Stats.WarCleric;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.WarCleric;

public class ActDivineFist : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatWarCleric;
    public override string PromotionString => Constants.WarClericId;
    public override int Cooldown => 5;
    public override int AbilityId => Constants.ActDivineFistId;

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        // Divine Descent Removes cost from this ability
        if (CC.HasCondition<ConDivineDescent>()) convertToMp.cost = 0;
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        Point originalPunched = TC.pos;
        bool divineDescentActive = CC.HasCondition<ConDivineDescent>();
        // Divine Descent Removes cooldown from this ability and increases damage from the melee attack by 30%.
        Condition solBlade = CC.AddCondition<ConShiningBlade>(1);
        // Perform a melee attack against the enemy.
        float dmgMulti = 1.0F;
        if (divineDescentActive) dmgMulti = 1.3F;
        new ActMeleeDivineFist
        {
            DamageMultiOverride = dmgMulti
        }.Perform(CC, TC);

        solBlade.Kill();
        // Render Holy Arrows flying at nearby targets
        int boltCount = 4;

        int power = GetPower(CC);
        int healAmount = HelperFunctions.SafeDice("ActDivineFist", power);
        foreach (Chara target in pc.currentZone.map.ListCharasInCircle(TC.pos, 3F))
        {
            if (boltCount <= 0) break;
            // Doesn't proc on user or original target.
            if (target == TC || target == CC) continue;
            if (target.IsHostile(CC))
            {
                ActEffect.DamageEle(CC, EffectId.Arrow, power, Element.Create(Constants.EleHoly, power / 10), new List<Point>
                {
                    target.pos
                }, new ActRef
                {
                    act = this
                });
            }
            else
            {
                target.HealHP(healAmount, HealSource.Magic);
            }
            ElementRef elementRef = setting.elements["eleHoly"];
            Effect arrowEffect = Effect.Get("spell_arrow");
            arrowEffect.sr.color = elementRef.colorSprite;
            TrailRenderer componentInChildren = arrowEffect.GetComponentInChildren<TrailRenderer>();
            Color startColor = componentInChildren.endColor = elementRef.colorSprite;
            componentInChildren.startColor = startColor;
            arrowEffect.Play(originalPunched, 0f, target.pos);
            boltCount--;
        }

        if (!divineDescentActive)
        {
            CC.AddCooldown(AbilityId, Cooldown);
        }
        return true;
    }
}