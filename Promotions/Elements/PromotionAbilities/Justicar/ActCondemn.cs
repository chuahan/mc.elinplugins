using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Justicar;

/// <summary>
///     Justicar Ability
///     Condemn targets a point and attempts to entangle all enemies around that area, dealing damage.
///     For each target in the area, add Protection to the Justicar and all allies around them.
/// </summary>
public class ActCondemn : Ability
{
    private float _effectRadius = 3F;

    public override bool CanPerform()
    {
        if (CC.MatchesPromotion(Constants.FeatJusticar))
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
        // Can I play SFX Chains here?
        int condemnedTargets = 0;
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(TP, _effectRadius, CC, false, true))
        {
            ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
            {
                origin = CC.Chara,
                n1 = nameof(ConEntangle)
            });

            // Inflict Bane.
            ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
            {
                origin = CC.Chara,
                n1 = nameof(ConBane)
            });

            int damage = HelperFunctions.SafeDice(Constants.CondemnAlias, GetPower(CC));
            target.DamageHP(damage, AttackSource.Melee, CC);
            condemnedTargets++;
        }

        int protectionAmount = condemnedTargets * GetPower(CC);
        foreach (Chara ally in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, true))
        {
            ConProtection? protection = (ConProtection)(ally.GetCondition<ConProtection>() ?? ally.AddCondition<ConProtection>());
            protection?.AddProtection(protectionAmount);
        }

        return true;
    }
}