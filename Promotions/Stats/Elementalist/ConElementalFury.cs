using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities.Elementalist;
namespace PromotionMod.Stats;

public class ConElementalFury : BaseBuff
{
    [JsonProperty(PropertyName = "E")] public Dictionary<int, int> ElementalStockpile = new Dictionary<int, int>();

    public override void Tick()
    {
        // Will not persist in regions.
        if (_zone.IsRegion)
        {
            Kill();
        }

        List<Chara> potentialTargets = HelperFunctions.GetCharasWithinRadius(owner.pos, 3f, owner, false, true);
        if (potentialTargets.Count != 0)
        {
            // Pick up 3 elements to cut nearby enemies every tick.
            for (int i = 0; i < 3; i++)
            {
                int element = ElementalStockpile.Keys.RandomItem();
                if (element == 0)
                {
                    // If the stockpile was depleted, end the Fury.
                    owner.Say("elementalist_fury_end");
                    Kill();
                }

                int powerAmped = power * ElementalStockpile[element];
                ActRef actRef = default(ActRef);
                actRef.act = new ActElementalFury();
                actRef.origin = owner;
                actRef.aliasEle = Constants.ElementAliasLookup[element];
                Element eleObj = Element.Create(actRef.aliasEle, powerAmped / 10);
                
                foreach (Chara target in potentialTargets.OfType<Chara>())
                {
                    ActEffect.DamageEle(owner, EffectId.Sword, powerAmped, eleObj, new List<Point>()
                    {
                        target.pos
                    }, actRef, nameof(ActElementalFury)); // TODO: Text for Elemental Fury
                    //ActEffect.ProcAt(EffectId.Sword, powerAmped, BlessedState.Normal, owner, target, target.pos, true, actRef);
                }

                // After cutting all enemies, remove that element from the stockpile.
                ElementalStockpile.Remove(element);
            }
        }
        else
        {
            // If no targets nearby lose a random element.
            int element = ElementalStockpile.Keys.RandomItem();
            if (element == 0)
            {
                // If the stockpile was depleted, end the Fury.
                owner.Say("elementalist_fury_end");
                Kill();
            }
            ElementalStockpile.Remove(element);
        }
    }
}