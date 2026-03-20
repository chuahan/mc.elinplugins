using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Justicar;

/// <summary>
///     Justicar Ability
///     Condemn targets a point and attempts to entangle all enemies around that area, dealing damage.
///     For each target in the area, add Protection to the Justicar and all allies around them.
///     If you are good aligned, you will boost the critical chance and damage of nearby allies.
///     If you are evil aligned, your chains can inflict burning.
/// </summary>
public class ActCondemn : Ability
{
    private float _effectRadius = 3F;

    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatJusticar))
        {
            if (CC.IsPC) Msg.Say("classlocked_ability".lang(Constants.JusticarId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override void OnMarkMapHighlights()
    {
        if (!scene.mouseTarget.pos.IsValid)
        {
            return;
        }
        List<Point> list = _map.ListPointsInCircle(scene.mouseTarget.pos, _effectRadius);
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
        // Get Karma Scores for the Player.
        // NPCs will be considered 0 Karma.
        bool positiveKarma = false, negativeKarma = false;
        if (CC.IsPCFactionOrMinion || CC.IsPC)
        {
            // For PC Faction, use PC's Karma.
            positiveKarma = player.karma >= 0;
            negativeKarma = player.karma < 0;
        }

        int calcPower = GetPower(CC);
        
        // Can I play SFX Chains here?
        int condemnedTargets = 0;
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(TP, _effectRadius, CC, false, true))
        {
            ActEffect.ProcAt(EffectId.Debuff, calcPower, BlessedState.Normal, CC, target, target.pos, true, new ActRef
            {
                origin = CC.Chara,
                n1 = nameof(ConEntangle)
            });

            // Inflict Bane.
            ActEffect.ProcAt(EffectId.Debuff, calcPower, BlessedState.Normal, CC, target, target.pos, true, new ActRef
            {
                origin = CC.Chara,
                n1 = nameof(ConBane)
            });

            int damage = HelperFunctions.SafeDice(Constants.CondemnAlias, calcPower);
            target.DamageHP(damage, AttackSource.Melee, CC);
            condemnedTargets++;

            if (negativeKarma)
            {
                ActEffect.ProcAt(EffectId.Debuff, calcPower, BlessedState.Normal, Act.CC, target, target.pos, false, new ActRef
                {
                    origin = Act.CC.Chara,
                    n1 = nameof(ConBurning)
                });
            }
        }

        int protectionAmount = condemnedTargets * calcPower;
        foreach (Chara ally in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, true))
        {
            ConProtection? protection = (ConProtection)(ally.GetCondition<ConProtection>() ?? ally.AddCondition<ConProtection>());
            protection?.AddProtection(protectionAmount);
            
            if (positiveKarma)
            {
                int boostPower = (int)(HelperFunctions.SigmoidScaling(calcPower, 10, 50));
                ally.AddCondition(SubPoweredCondition.Create(nameof(ConCritBoost), calcPower, boostPower));
            }
        }

        return true;
    }
}