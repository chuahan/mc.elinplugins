using System.Collections.Generic;
using Cwl.Helper.Extensions;
using UnityEngine;
namespace PromotionMod.Elements.PromotionFeats;

public abstract class PromotionFeat : Feat
{
    public abstract string PromotionClassId { get; }
    public abstract int PromotionClassFeatId { get; }

    public abstract List<int> PromotionAbilities { get; }
    
    // Tourists are able to Promote into any of these classes.
    private bool IsTourist => owner.Chara?.c_idJob == "tourist";
    public override Sprite GetIcon(string suffix = "")
    {
        return SpriteSheet.Get(source.alias);
    }

    // Promotions require specific base classes.
    protected abstract bool Requirement();

    // Add Promotion Class specific Skills/Potentials.
    protected abstract void ApplyInternal();

    // Add NPC Specific Class applications. This involves picking which abilities to add with weights.
    protected abstract void ApplyInternalNPC(Chara c);

    internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        if (!Requirement() || IsTourist || !PromotionMod.Debug)
        {
            owner.Remove(id);
            foreach (int abilityId in PromotionAbilities)
            {
                owner.Remove(abilityId);
            }
        }
        else
        {
            ApplyInternal();
            if (owner.Chara.IsPC)
            {
                foreach (int abilityId in PromotionAbilities)
                {
                    owner.Chara.AddElement(abilityId, 0);
                }
            }
            else
            {
                ApplyInternalNPC(owner.Chara);
            }
        }
    }
}