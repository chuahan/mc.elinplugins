using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Justicar;

/// <summary>
///     Justicar Ability
///     The Justicar inflicts Armor Break and Excommunication on the target.
///     All enemies near the target are afflicted with fear.
/// </summary>
public class ActIntimidate : Ability
{
    private float _effectRadius = 3F;

    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatJusticar))
        {
            Msg.Say("classlocked_ability".lang(Constants.JusticarId.lang()));
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
            // Highlight the target point a different color.
            if (Equals(scene.mouseTarget.pos, item))
            {
                item.SetHighlight(7);
            }
            else
            {
                item.SetHighlight(8);
            }
        }
    }

    public override bool Perform()
    {
        int breakAmount = (int)HelperFunctions.SigmoidScaling(GetPower(CC), 10, 25);
        TC.Chara.AddCondition(SubPoweredCondition.Create(nameof(ConArmorBreak), GetPower(CC), breakAmount));

        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(TC.pos, _effectRadius, CC, false, false))
        {
            // Inflict AOE Bane and Fear
            ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
            {
                origin = CC.Chara,
                n1 = nameof(ConBane)
            });

            ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
            {
                origin = CC.Chara,
                n1 = nameof(ConFear)
            });
        }

        return true;
    }
}