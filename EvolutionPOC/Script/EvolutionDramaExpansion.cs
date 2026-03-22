using System;
using System.Collections.Generic;
using System.Linq;
using Cwl.API.Drama;
using Cwl.Helper;
using Cwl.Helper.Extensions;
namespace Evolution;

internal class EvolutionDramaExpansion : DramaOutcome
{
    private static bool CanEvolve(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out var actor);
        (bool evolvable, string evoChara, string evoThing) = actor.IsEvolvable();
        if (evolvable) actor.SetFlagValue("canEvo", 1);
        return false;
    }
    
    private static bool Evolve(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        dm.RequiresActor(out var actor);
        (bool evolvable, string evoChara, string evoThing) = actor.IsEvolvable();
        
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

        // Split and delete the evolution item from PC's inventory.
        Thing currentItem = allStorage.First(thing => thing.id.Equals(evoThing, StringComparison.InvariantCulture));
        Thing deleteTarget = currentItem.Split(1);
        deleteTarget.Destroy();

        // Create the evolved character.
        Chara evolvedChara = CharaGen.Create(evoChara);
        
        // Take all the items from the old character and throw them into the new character's inventory.
        foreach (Thing posession in actor.things)
        {
            evolvedChara.AddThing(posession);
        }
        
        // Copy original elements and max them against the new form.
        foreach (KeyValuePair<int, Element> element in actor.elements.dict)
        {
            Element copiedElement = evolvedChara.elements.GetOrCreateElement(element.Key);
            copiedElement.vBase = Math.Max(copiedElement.vBase, element.Value.vBase);
            copiedElement.vLink = Math.Max(copiedElement.vLink, element.Value.vLink);
            copiedElement.vSource = Math.Max(copiedElement.vSource, element.Value.vSource);
        }

        // Sync Metadata
        evolvedChara.bio = actor.bio;
        evolvedChara._hobbies = actor._hobbies;
        evolvedChara._ability = actor._ability;
        evolvedChara._tactics = actor._tactics;
        evolvedChara._job = actor._job;
        evolvedChara._works = actor._works;
        evolvedChara.hp = evolvedChara.MaxHP;
        evolvedChara.mana.Set(evolvedChara.mana.max);
        evolvedChara._affinity = actor._affinity;
        
        EClass.pc.currentZone.AddCard(evolvedChara, actor.pos);
        EClass._zone.branch.AddMemeber(evolvedChara);
        EClass.pc.party.AddMemeber(evolvedChara);

        evolvedChara.PlayEffect("aura_heaven");
        Msg.Say("evolution_complete".langGame(actor.NameSimple, actor.Name, evolvedChara.Name));
        
        // Delete the original character
        EClass._zone.map._RemoveCard(actor);
        actor.RemoveGlobal();

        return true;
    }
}