using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Dancer;
using UnityEngine;
namespace PromotionMod.Elements.Alraune;

/// <summary>
/// Alraune stance that applies  and Drunkeness to targets repeatedly.
/// </summary>
public class AuraSweetScent : ConAura
{
    public override void OnStart()
    {
        owner.ShowEmo(Emo.love);
    }
    
    public override string TextDuration => "";
    
    public override bool CanManualRemove => false;
    
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    public override AuraType AuraTarget => AuraType.Foe;

    public override int IdAbility => Constants.StSweetScentId;

    public override void ApplyFoe(Chara target)
    {
        if (!target.HasCondition<ConInfatuation>())
        {
            ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, CC, target, target.pos, true, new ActRef
            {
                origin = CC,
                n1 = nameof(ConInfatuation)
            });

            // Also add Drunk.
            target.AddCondition<ConDrunk>();
        }
    }
}