using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements;

public class FeatNaga : Feat
{
    virtual internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        if (hint) return;
        
        if (add == 1)
        {
            if (owner.Chara.IsPC)
            {
                if (!owner.Chara.HasElement(Constants.ActSerpentineConstriction)) owner.Chara.elements.SetBase(Constants.ActSerpentineConstriction, 1);
                if (!owner.Chara.HasElement(Constants.ActSerpentineAgility)) owner.Chara.elements.SetBase(Constants.ActSerpentineAgility, 1);

                // This is the process used to add a permanent spell to the PC.
                owner.Chara._listAbility ??= new List<int>();
                owner.Chara._listAbility.Add(SPELL.weapon_Poison);
                if (owner.Chara.elements.GetElement(SPELL.weapon_Poison) == null) owner.Chara.elements.ModBase(SPELL.weapon_Poison, 1);
            }
            else
            {
                if (!owner.Chara.ability.Has(Constants.ActSerpentineConstriction)) owner.Chara.ability.Add(Constants.ActSerpentineConstriction, 100, false);
                if (!owner.Chara.ability.Has(Constants.ActSerpentineAgility)) owner.Chara.ability.Add(Constants.ActSerpentineAgility, 100, false);
                owner.Chara.ability.Add(SPELL.weapon_Poison, 100, false);
            }
        }
        else if (add == -1)
        {
            if (owner.Chara.IsPC)
            {
                owner.Chara.elements.Remove(Constants.ActSerpentineConstriction);
                owner.Chara.elements.Remove(Constants.ActSerpentineAgility);
                
                // This is the process to remove a permanent spell from the PC.
                owner.Chara._listAbility.Remove(id);
                if (owner.Chara._listAbility.Count == 0) owner.Chara._listAbility = (List<int>) null;
            }
            else
            {
                if (owner.Chara.ability.Has(Constants.ActSerpentineConstriction)) owner.Chara.ability.Remove(Constants.ActSerpentineConstriction);
                if (owner.Chara.ability.Has(Constants.ActSerpentineAgility)) owner.Chara.ability.Remove(Constants.ActSerpentineAgility);
                owner.Chara.ability.Remove(SPELL.weapon_Poison);
            }
        }
    }
}