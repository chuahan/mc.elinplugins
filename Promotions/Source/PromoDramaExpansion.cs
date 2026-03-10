using System.Collections.Generic;
using System.Linq;
using Cwl.API.Drama;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Source;

internal class PromoDramaExpansion : DramaOutcome
{
    private const int KumiBookPoints = 50;
    
    private static bool SetPromoFlag(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Set Affinity Flags
        PromoDramaExpansion.SetAffinityFlags(Constants.LailahCharaId);
        PromoDramaExpansion.SetAffinityFlags(Constants.MinariCharaId);
        PromoDramaExpansion.SetAffinityFlags(Constants.VyersCharaId);

        // Set Deciphering Status
        PromoDramaExpansion.SetDecipheringStatus();
        
        // Set Lailah Quest Requirements:
        //EClass.player.dialogFlags["lailahFriendship1Available"];
        //EClass.player.dialogFlags["lailahFriendship2Available"];
        //EClass.player.dialogFlags["lailahFriendship3Available"];
        //EClass.player.dialogFlags["lailahFriendship4Available"];
        //EClass.player.dialogFlags["lailahFriendship1Complete"];
        //EClass.player.dialogFlags["lailahFriendship2Complete"];
        //EClass.player.dialogFlags["lailahFriendship3Complete"];
        //EClass.player.dialogFlags["lailahFriendship4Complete"];
                
        // Lailah Friendship Quests
        // Reach Friendship 50 To Trigger lailahFriendship1.
        // Wait 1 day
        // Trigger lailahFriendship1Followup.
        
        // Reach Friendship 50 to trigger lailahFriendship2.
        
        // Complete Minari's Questline.
        // Trigger Minari's Golem Search Questline.
        // Talk to Vyers
        // Reach Friendship 75 To trigger lailahFriendship3.
        
        // Talk to either Minari OR Vyers to trigger the laliahFriendship4
        
        return true;
    }

    private static void SetDecipheringStatus()
    {
        EClass.player.dialogFlags.TryGetValue("lailahManualDeciphered", out int lailahManualDeciphered);
        bool manualDeciphered = lailahManualDeciphered == 1;
        if (!manualDeciphered)
        {
            EClass.player.dialogFlags.TryGetValue("lailahDeciphering", out int decipheringProgress);
            EClass.player.dialogFlags[$"lailahDecipherStatus1"] = 0;
            EClass.player.dialogFlags[$"lailahDecipherStatus2"] = 0;
            EClass.player.dialogFlags[$"lailahDecipherStatus3"] = 0;
            EClass.player.dialogFlags[$"lailahDecipherStatus4"] = 0;
            switch (decipheringProgress)
            {
                case >= 75:
                    EClass.player.dialogFlags[$"lailahDecipherStatus4"] = 1;
                    break;
                case >= 50:
                    EClass.player.dialogFlags[$"lailahDecipherStatus3"] = 1;
                    break;
                case >= 25:
                    EClass.player.dialogFlags[$"lailahDecipherStatus2"] = 1;
                    break;
                default:
                    EClass.player.dialogFlags[$"lailahDecipherStatus1"] = 1;
                    break;
            }
        }
        else
        {
            EClass.player.dialogFlags[$"lailahDecipherStatus1"] = 0;
            EClass.player.dialogFlags[$"lailahDecipherStatus2"] = 0;
            EClass.player.dialogFlags[$"lailahDecipherStatus3"] = 0;
            EClass.player.dialogFlags[$"lailahDecipherStatus4"] = 0;
            
            EClass.player.dialogFlags.TryGetValue("lailahWorking", out int lailahWorking);
            if (lailahWorking == 1)
            {
                EClass.player.dialogFlags.TryGetValue("lailahDecipheringTimer", out int lailahDecipheringTimer);
                if (lailahDecipheringTimer >= 1 && EClass.world.date.GetRaw() > lailahDecipheringTimer)
                {
                    EClass.player.dialogFlags["lailahManualReady"] = 1;
                }   
            }
        }
    }
    
    private static void SetAffinityFlags(string charaId)
    {
        Chara dramaCharacter = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == charaId);
        if (dramaCharacter != null)
        {
            int charaAffinity = dramaCharacter._affinity;
            EClass.player.dialogFlags[$"{charaId}Friendship"] = charaAffinity switch
            {
                >= 75 => 3,
                >= 50 => 2,
                >= 25 => 1,
                _ => 0
            };
        }
    }
    
    private static bool CheckManualMaterials(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        EClass.player.dialogFlags.TryGetValue("lailahDeciphering", out int decipheringProgress);
                
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
            Thing currentItem = allStorage[i];
            // Normal Ancient Books are worth 1 each.
            if (currentItem is { id: "book_ancient", IsIdentified: true })
            {
                while (decipheringProgress < 100 && !currentItem.isDestroyed)
                {
                    if (currentItem.Num > 1)
                    {
                        Thing turnIn = currentItem.Split(1);
                        turnIn.Destroy();
                    }
                    else
                    {
                        currentItem.Destroy();
                    }
                    decipheringProgress++;
                }
            }
            
            // Kumi books are worth 50 each.
            if (currentItem is { id: "book_kumiromi", IsIdentified: true })
            {
                while (decipheringProgress < 100 && !currentItem.isDestroyed)
                {
                    if (currentItem.Num > 1)
                    {
                        Thing turnIn = currentItem.Split(1);
                        turnIn.Destroy();
                    }
                    else
                    {
                        currentItem.Destroy();
                    }
                    decipheringProgress += 50;
                }
            }
            
            if (decipheringProgress >= 100) break;
        }

        EClass.player.dialogFlags[$"lailahDeciphering"] = decipheringProgress;
        PromoDramaExpansion.SetDecipheringStatus();
        return decipheringProgress >= 100;
    }
    
    private static bool SetEventTimer(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Store the current time onto the player.
        parameters.Requires(out var timerName, out var timerLength);
        EClass.player.dialogFlags[timerName] = EClass.world.date.GetRaw() + timerLength.AsInt(4320); // 3 Days, 1440 turns a day.
        return false;
    }

    private static bool HasNoManualMaterials(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
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
        
        bool hasBookMaterial = !allStorage.Any(thing => thing.id is "book_ancient" or "book_kumiromi");
        return hasBookMaterial;
    }
}