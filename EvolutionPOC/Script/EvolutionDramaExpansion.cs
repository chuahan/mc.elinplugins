using System;
using System.Collections.Generic;
using System.Linq;
using Cwl.API.Drama;
using Cwl.Helper.Extensions;
namespace Evolution;

internal class EvolutionDramaExpansion : DramaOutcome
{
    private static bool Evolve(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out var actor);
        (bool evolvable, string charaResult, string evoThing) = actor.IsEvolvable();
        
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

        // Split and delete the item.
        Thing currentItem = allStorage.First(thing => thing.id.Equals(evoThing, StringComparison.InvariantCulture));
        Thing deleteTarget = currentItem.Split(1);
        deleteTarget.Destroy();

        // Calculate the skill and attribute gains of the character being evolved vs it's base.
        // Take all the items from the old character and throw them into the player's inventory.
        // Delete the character
        // Create the evolved character and add to the party.
        // Apply the skill and attribute gains.
        
        return true;
    }
}