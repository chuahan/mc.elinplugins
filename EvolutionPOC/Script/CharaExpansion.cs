using System;
using System.Collections.Generic;
using System.Linq;
using Cwl.Helper.Extensions;
namespace Evolution;

public static class CharaExpansion
{
    /// <summary>
    /// Returns the data needed for Evolution.
    /// Requires the Chara to have a tag in the source sheet of evolve#chara_id#thing_id
    /// </summary>
    /// <param name="target">Chara to evaluate</param>
    /// <returns>Evolvable or not, the evolution result chara id, the evolution item required</returns>
    /// <exception cref="Exception">If evolution tag is malformed will complain.</exception>
    public static (bool, string, string) IsEvolvable(this Chara target)
    {
        bool hasItem = false;
        bool extraReq = false;
        foreach (var tag in target.source.tag) {
            if (tag.StartsWith("evolve#"))
            {
                var evolutionParams = tag.Split("#");
                if (evolutionParams.Length != 3)
                    throw new Exception("Evolution Tag should be in the form of evolve#chara_id#thing_id");
                
                // Locate the required evolution heart.
                // Search in Inventory
                List<Thing> allStorage = new List<Thing>();
                allStorage.AddRange(EClass.pc.things);
                Thing toolbelt = EClass.pc.things.Find(x => x.trait is TraitToolBelt);

                // Search in Toolbelt.
                allStorage.AddRange(EClass.pc.things.Find(x => x.trait is TraitToolBelt).things);

                // Storage in Inventory
                List<Thing> inventoryStorage = EClass.pc.things.FindAll(x => x.trait is TraitContainer);
                foreach (Thing inventoryStorageContainer in inventoryStorage)
                {
                    allStorage.AddRange(inventoryStorageContainer.things);
                }

                // Storage in Toolbelt
                List<Thing> toolBeltStorage = toolbelt.things.FindAll(x => x.trait is TraitContainer);
                foreach (Thing toolBeltContainer in toolBeltStorage)
                {
                    allStorage.AddRange(toolBeltContainer.things);
                }

                for (int i = 0; i < allStorage.Count; i++)
                {
                    hasItem = allStorage.Any(thing => thing.id.Equals(evolutionParams[2]));
                }
                
                extraReq = target._affinity >= 75;
                
                return (extraReq && hasItem, evolutionParams[1], evolutionParams[2]);
            }
        }
        
        return (false, "", "");
    }
}