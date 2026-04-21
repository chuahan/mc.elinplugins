using System;
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
    public bool IsTourist => owner.Chara.job.id.Equals("tourist", StringComparison.InvariantCultureIgnoreCase);

    public virtual string JobRequirement => "tourist";
    public override Sprite GetIcon(string suffix = "")
    {
        return SpriteSheet.Get(source.alias);
    }

    // Promotions require specific base classes.
    protected virtual bool Requirement()
    {
        // Most classes just a class check.
        return owner.Chara.job.id.Equals(JobRequirement, StringComparison.InvariantCultureIgnoreCase) || IsTourist;
    }

    // Add NPC Specific Class applications. This involves picking which abilities to add with weights.
    protected abstract void ApplyInternalNPC(Chara c);

    virtual internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        if (!hint)
        {
            if (!Requirement() && !IsTourist)
            {
                owner.Remove(id);
                foreach (int abilityId in PromotionAbilities)
                {
                    owner.Remove(abilityId);
                }
            }
            else
            {
                if (owner.Chara.IsPC)
                {
                    foreach (int abilityId in PromotionAbilities)
                    {
                        if (!owner.Chara.HasElement(abilityId))
                        {
                            owner.Chara.AddElement(abilityId);
                        }
                    }
                }
                else
                {
                    ApplyInternalNPC(owner.Chara);
                }
            }
        }
    }

    virtual internal void Demote(Chara c)
    {
        c.SetFeat(PromotionClassFeatId, 0);

        foreach (int abilityId in PromotionAbilities)
        {
            // This should handle NPC abilities.
            if (c.ability.Has(abilityId))
            {
                c.ability.Remove(abilityId);
            }

            // PC Abilities are a bit more stubborn.
            if (c.IsPC)
            {
                c.elements.Remove(abilityId);
            }
        }
        if (c.IsPC)
        {
            LayerAbility.Redraw();
        }
    }

    public static int GetAdditionalSpellElement(Chara c)
    {
        // If the character promoting has a main element, they will gain spells in that element.
        // If no main element but has domain, take the first element in the domain list.
        // The element Ids need to be set to 0 Index to map them to the spells. 0 is fire to void as 26.
        int eleCode = 11; // Defaults to Magic
        if (c.MainElement != Void)
        {
            eleCode = c.MainElement.id - 910;
        }
        else if (c.job.domain.Length != 0)
        {
            eleCode = c.job.domain[0] - 910;
        }

        return eleCode;
    }
}