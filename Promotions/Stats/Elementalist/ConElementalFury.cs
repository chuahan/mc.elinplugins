using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PromotionMod.Common;
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
                // If the stockpile was depleted, end the Fury.
                if (ElementalStockpile.Count == 0) Kill();
                int element = ElementalStockpile.Keys.RandomItem();
                int powerAmped = power * ElementalStockpile[element];
                ActRef actRef = default(ActRef);
                actRef.origin = owner;
                actRef.aliasEle = Constants.ElementAliasLookup[element];

                foreach (Chara target in potentialTargets.OfType<Chara>())
                {
                    ActEffect.ProcAt(EffectId.Sword, powerAmped, BlessedState.Normal, owner, target, target.pos, true, actRef);
                }

                // After cutting all enemies, remove that element from the stockpile.
                ElementalStockpile.Remove(element);
            }
        }
    }
}