using System.Collections.Generic;
using System.Linq;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using Cwl.Helper.Extensions;

namespace BardMod.Source;

internal class DramaExpansion : DramaOutcome
{
    static bool set_bard_flags(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        Chara niyon = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.NiyonCharaId);
        Chara selena = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.SelenaCharaId);
        if (selena is not null)
        {
            // Set Selena Friendship Affinity Flags
            int selenaAffinity = selena._affinity;
            EClass.player.dialogFlags["selenaFriendship"] = selenaAffinity switch
            {
                >= 75 => 3,
                >= 50 => 2,
                >= 25 => 1,
                _ => 0
            };
        }

        if (niyon is not null)
        {
            // Set Niyon Friendship Affinity Flags
            int niyonAffinity = niyon._affinity;
            EClass.player.dialogFlags["niyonFriendship"] = niyonAffinity switch
            {
                >= 75 => 3,
                >= 50 => 2,
                >= 25 => 1,
                _ => 0
            };
        }
        
        // 10 Music to Start the Bard Quest.
        EClass.player.dialogFlags["bardMusicRequirement"] = EClass.pc.Evalue(Constants.MusicSkill) / 10;

        // Pianist Class to Change Dialogue.
        EClass.player.dialogFlags["bardPianist"] = EClass.pc.c_idJob == "pianist" ? 1 : 0;

        // Bard Cumulative Skill Levels to determine skill progression.
        int bardSkillLevel = HelperFunctions.GetCumulativeBardSkillLevel();
        EClass.player.dialogFlags["bardSkillLevel"] = bardSkillLevel switch
        {
            >= 30 => 3,
            >= 20 => 2,
            >= 10 => 1,
            _ => 0
        };
        int bardSkillTier = EClass.player.dialogFlags["bardSkillLevel"];

        // Fetch other dialogue flags to set states
        // Niyon Flags
        EClass.player.dialogFlags.TryGetValue("niyonIntroComplete", out int niyonIntroComplete);
        EClass.player.dialogFlags.TryGetValue("niyonFriendship", out int niyonFriendship);
        EClass.player.dialogFlags.TryGetValue("niyonRecruited", out int niyonRecruited);
        EClass.player.dialogFlags.TryGetValue("niyonAwakened", out int niyonAwakened);
        EClass.player.dialogFlags.TryGetValue("niyonAwakeningInProgress", out int niyonAwakeningInProgress);
        EClass.player.dialogFlags.TryGetValue("niyonTier0SongsLearned", out int niyonTier0SongsLearned);
        EClass.player.dialogFlags.TryGetValue("niyonTier1SongsLearned", out int niyonTier1SongsLearned);
        EClass.player.dialogFlags.TryGetValue("niyonTier2SongsLearned", out int niyonTier2SongsLearned);
        EClass.player.dialogFlags.TryGetValue("niyonPowersComplete", out int niyonPowersComplete);
        EClass.player.dialogFlags.TryGetValue("niyonBackgroundComplete", out int niyonBackgroundComplete);
        EClass.player.dialogFlags.TryGetValue("niyonHasBardShop", out int niyonHasBardShop);

        // Selena Flags
        EClass.player.dialogFlags.TryGetValue("selenaIntroComplete", out int selenaIntroComplete);
        EClass.player.dialogFlags.TryGetValue("selenaFriendship", out int selenaFriendship);
        EClass.player.dialogFlags.TryGetValue("selenaRecruited", out int selenaRecruited);
        EClass.player.dialogFlags.TryGetValue("selenaAwakened", out int selenaAwakened);
        EClass.player.dialogFlags.TryGetValue("selenaAwakeningInProgress", out int selenaAwakeningInProgress);
        
        EClass.player.dialogFlags.TryGetValue("selenaTier0SongsLearned", out int selenaTier0SongsLearned);
        EClass.player.dialogFlags.TryGetValue("selenaTier1SongsLearned", out int selenaTier1SongsLearned);
        EClass.player.dialogFlags.TryGetValue("selenaTier2SongsLearned", out int selenaTier2SongsLearned);
        EClass.player.dialogFlags.TryGetValue("selenaBackgroundComplete", out int selenaBackgroundComplete);
        EClass.player.dialogFlags.TryGetValue("selenaTraumaComplete", out int selenaTraumaComplete);

        // Collab Flags
        EClass.player.dialogFlags.TryGetValue("niyonSelenaCollabUnlocked", out int niyonSelenaCollabUnlocked);
        EClass.player.dialogFlags.TryGetValue("niyonSelenaCollabComplete", out int niyonSelenaCollabComplete);
        EClass.player.dialogFlags.TryGetValue("niyonHearsSelena", out int niyonHearsSelena);
        EClass.player.dialogFlags.TryGetValue("selenaLearnsAboutNiyon", out int selenaLearnsAboutNiyon);

        // Niyon Bard Quest Progression
        // Tier 1 Spells - Unlocked after Intro + Level 10 Bard Skills
        EClass.player.dialogFlags["niyonLearnTier1Songs"] =
                (niyonTier0SongsLearned == 1 && bardSkillTier >= 1 && (niyonTier1SongsLearned == 0 || EClass.pc.Evalue(Constants.BardMagicSongId) == 0)) ? 1 : 0;
        // Bard Shop - Opens after you've learned Tier1 Songs + Level 20 Bard Skills
        EClass.player.dialogFlags["niyonCanOpenBardShop"] = (niyonTier1SongsLearned == 1 && bardSkillTier >= 2 && niyonHasBardShop == 0) ? 1 : 0;
        // Tier 2 spells - Unlocked after Collab was completed.
        EClass.player.dialogFlags["niyonNeedsSelenaTier2"] = (niyonSelenaCollabComplete == 0 &&
                                                              niyonTier1SongsLearned == 1 &&
                                                              bardSkillTier >= 2) ? 1 : 0; 
        EClass.player.dialogFlags["niyonLearnTier2Songs"] = (niyonSelenaCollabComplete == 1 &&
                                                             niyonTier1SongsLearned == 1 &&
                                                             bardSkillTier >= 2 &&
                                                             (niyonTier2SongsLearned == 0 || EClass.pc.Evalue(Constants.BardLuckSongId) == 0)) ? 1 : 0;
        // Niyon has no new content.
        EClass.player.dialogFlags["niyonNothingNew"] = (niyonHasBardShop == 1 && niyonTier2SongsLearned == 1 && niyonTier1SongsLearned == 1) ? 1 : 0;

        // Niyon Dialogue Stages
        // Niyon can talk about her powers if at least 25 affinity.
        EClass.player.dialogFlags["niyonPowersAvailable"] = (niyonFriendship >= 1 && niyonPowersComplete == 0) ? 1 : 0;
        // Niyon can talk about her background if at least 50 affinity, and you have talked to her about her powers.
        EClass.player.dialogFlags["niyonBackgroundAvailable"] = (niyonFriendship >= 2 && niyonPowersComplete == 1 && niyonBackgroundComplete == 0) ? 1 : 0;
        // Niyon can be recruited if fully befriended, Background learned, and Player has learned Tier 1 Bard Spells.
        EClass.player.dialogFlags["niyonRecruitAvailable"] = (niyonFriendship >= 3 && niyonBackgroundComplete == 1 && niyonTier1SongsLearned == 1 && niyonRecruited == 0) ? 1 : 0;
        // Niyon can be awakened after both her and Selena have been recruited, and the Collab was complete.
        EClass.player.dialogFlags["niyonAwakeningAvailable"] = (
            niyonTier2SongsLearned == 1 &&
            niyonRecruited == 1 &&
            niyonFriendship == 3 &&
            selenaRecruited == 1 &&
            selenaFriendship == 3 &&
            niyonSelenaCollabComplete == 1 &&
            niyonAwakeningInProgress == 0 &&
            niyonAwakened == 0) ? 1 : 0;
        if (selena is not null)
        {
            EClass.player.dialogFlags["niyonAwakeningSelenaInParty"] = (EClass.player.dialogFlags["niyonAwakeningAvailable"] == 1 && selena.IsPCParty) ? 1 : 0;
        }

        // Selena Bard Quest Progression
        // Tier 1 Spells - Unlocks after Intro + Level 10 Bard Skills.
        EClass.player.dialogFlags["selenaLearnTier1Songs"] =
                (selenaTier0SongsLearned == 1 && bardSkillTier >= 1 && (selenaTier1SongsLearned == 0 || EClass.pc.Evalue(Constants.BardChaosSongId) == 0)) ? 1 : 0;
        // Tier 2 spells - Unlocked after Collab was completed + Level 20 Bard Skills
        EClass.player.dialogFlags["selenaNeedsNiyonTier2"] = (niyonSelenaCollabComplete == 0 &&
                                                              selenaTier1SongsLearned == 1 &&
                                                              bardSkillTier >= 2) ? 1 : 0;
        EClass.player.dialogFlags["selenaLearnTier2Songs"] = (niyonSelenaCollabComplete == 1 &&
                                                              selenaTier1SongsLearned == 1 &&
                                                              bardSkillTier >= 2 &&
                                                              (selenaTier2SongsLearned == 0 || EClass.pc.Evalue(Constants.BardMirrorSongId) == 0)) ? 1 : 0;
        // Selena has no new content.
        EClass.player.dialogFlags["selenaNothingNew"] = (selenaTier2SongsLearned == 1 && selenaTier1SongsLearned == 1) ? 1 : 0;

        // Selena Dialogue Stages
        // Selena can talk about her background if at least 25 affinity.
        EClass.player.dialogFlags["selenaBackgroundAvailable"] = (selenaFriendship >= 1 && selenaBackgroundComplete == 0) ? 1 : 0;
        // Selena can talk about her trauma if at least 50 affinity, you have talked to her about her background, and you have told her about Niyon.
        EClass.player.dialogFlags["selenaTraumaAvailable"] = (selenaFriendship >= 2 && selenaBackgroundComplete == 1 && selenaTraumaComplete == 0 && selenaLearnsAboutNiyon == 1) ? 1 : 0;
        // Selena can be recruited if fully befriended, if you talked to her about her trauma, and Player has learned her Tier 1 Bard Spells.
        EClass.player.dialogFlags["selenaRecruitAvailable"] = (selenaFriendship >= 3 && selenaTraumaComplete == 1 && selenaTier1SongsLearned == 1 && selenaRecruited == 0) ? 1 : 0;
        // Selena can be awakened after she has been recruited, and you have learned all her bard magic.
        EClass.player.dialogFlags["selenaAwakeningAvailable"] = (selenaTier2SongsLearned == 1 && selenaRecruited == 1 && selenaFriendship == 3 && selenaAwakened == 0 && selenaAwakeningInProgress == 0) ? 1 : 0;
                
        // Collabs
        // To unlock the collab between Niyon and Selena:
        // Must have met both.
        // Triggers after niyonTier1SongUnlock
        // Speak to Niyon
        EClass.player.dialogFlags["niyonSelenaCollabUnlock"] = (niyonIntroComplete == 1 && selenaIntroComplete == 1 && niyonTier1SongsLearned == 1 && niyonHearsSelena == 0) ? 1 : 0;
        
        // Speak to Niyon with:
        // Niyon and Selena Friendship both at 3.
        // Niyon has heard Selena's music.
        // Selena must be recruited.
        // Selena must have been told about Niyon.
        // Selena is in the Party.
        if (selena is not null)
        {
            EClass.player.dialogFlags["niyonSelenaCollabAvailable"] = (niyonFriendship == 3 && selenaFriendship == 3 && niyonSelenaCollabUnlocked == 1 && selenaLearnsAboutNiyon == 1 && selena.IsPCParty && niyonSelenaCollabComplete == 0) ? 1 : 0;
        }
        
        // If the player has lost their instrument after initiating Bardship, but hasn't opened the shop yet.
        // If we have multiple instruments, search for the one with the flag IsSelectedInstrument.
        // Instruments can be anywhere in inventory, including substorage.

        // Search in Inventory
        List<Thing> allInstruments = pc.things.FindAll(x => x.trait is TraitToolBard);
		
        // Search in Toolbelt.
        Thing toolbelt = pc.things.Find(x => x.trait is TraitToolBelt);
        allInstruments.AddRange(toolbelt.things.FindAll(x => x.trait is TraitToolBard));
		
        // Storage in Inventory
        List<Thing> inventoryStorage = pc.things.FindAll(x => x.trait is TraitContainer);
        foreach (Thing inventoryStorageContainer in inventoryStorage)
        {
            allInstruments.AddRange(inventoryStorageContainer.things.FindAll(x => x.trait is TraitToolBard));
        }
		
        // Storage in Toolbelt
        List<Thing> toolBeltStorage = toolbelt.things.FindAll(x => x.trait is TraitContainer);
        foreach (Thing toolBeltContainer in toolBeltStorage)
        {
            allInstruments.AddRange(toolBeltContainer.things.FindAll(x => x.trait is TraitToolBard));
        }

        if (allInstruments.Count == 0 && niyonHasBardShop == 0 && niyonIntroComplete == 1) EClass.player.dialogFlags["niyonLostInstrument"] = 1; 
        
        return true;
    }

    static bool add_bardskills(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Assert must have 2 paremeters, string dictating which tutor is teaching the skills, and int dictating which tier of music to grant.
        if (parameters is not [{ } teacher, { } tier])
        {
            return false;
        }

        int songTier = int.Parse(tier);

        Dictionary<string, List<List<int>>> musicMap = new Dictionary<string, List<List<int>>>
        {
            {
                "niyon", new List<List<int>>
                {
                    // 0
                    new List<int>
                    {
                        Constants.BardStrengthSongId,
                        Constants.BardSpeedSongId,
                        Constants.BardHealingSongId,
                        Constants.BardPuritySongId,
                        Constants.BardFinaleSongId,
                    },
                    // 1
                    new List<int>
                    {
                        Constants.BardMagicSongId,
                        Constants.BardGuardSongId,
                        Constants.BardSleepSongId,
                        Constants.BardEchoSongId,
                    },
                    // 2
                    new List<int>
                    {
                        Constants.BardLuckSongId,
                        Constants.BardVigorSongId,
                        Constants.BardCheerSongId,
                        Constants.BardDisruptionSongId,
                        Constants.BardTuningSongId,
                    },

                }
            },
            {
                "selena", new List<List<int>>
                {
                    // 0
                    new List<int>
                    {
                        Constants.BardDishearteningSongId,
                        Constants.BardDisorientationSongId,
                        Constants.BardSlashSongId,
                        Constants.BardKnockbackSongId
                    },
                    // 1
                    new List<int>
                    {
                        Constants.BardChaosSongId,
                        Constants.BardWitheringSongId,
                        Constants.BardScathingSongId,
                        Constants.BardDispelSongId,
                    },
                    // 2
                    new List<int>
                    {
                        Constants.BardMirrorSongId,
                        Constants.BardShellSongId,
                        Constants.BardDrowningSongId,
                        Constants.BardWitchHuntSongId,
                        Constants.BardElementalSongId,
                    },
                }
            }
        };

        // Add all songs to the player.
        foreach (int song in musicMap[teacher][songTier])
        {
            player.chara.AddElement(song, 0);
        }
        // niyonTier0SongsLearned;
        string flagKey = teacher + "Tier" + songTier + "SongsLearned";
        EClass.player.dialogFlags[flagKey] = 1;
        return true;
    }

    static bool check_instrument_repair_materials(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Need 9 Mithril Ingots, 9 Rosewood Planks, 3 Sun Crystals

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

        // Search all storage for these items
        int metalCountNeeded = 9; // Need 9 Ingots
        int plankCountNeeded = 9; // Need 9 Planks
        int crystalCountNeeded = 9; // Need 3 Crystals
        int metalCount = 0;
        int plankCount = 0;
        int crystalCount = 0;
        foreach (Thing thing in allStorage)
        {
            if (thing is { id: "ingot", material.alias: "mithril", IsImportant: false })
            {
                metalCount += thing.Num;
            }
            if (thing is { id: "plank", material.alias: "rosewood", IsImportant: false })
            {
                plankCount += thing.Num;
            }
            if (thing is { id: "crystal_sun", IsImportant: false })
            {
                crystalCount += thing.Num;
            }
        }

        if (metalCount >= 9 && plankCount >= 9 && crystalCount >= 3)
        {
            // Go back through and remove the designated amounts.
            foreach (Thing thing in allStorage)
            {
                if (thing is { id: "ingot", material.alias: "mithril", IsImportant: false })
                {
                    if (metalCountNeeded > 0)
                    {
                        if (thing.Num > metalCountNeeded)
                        {
                            thing.Num -= metalCountNeeded;
                        }
                        else
                        {
                            metalCountNeeded -= thing.Num;
                            EClass.pc.things.Remove(thing);
                            EClass.pc.things.OnRemove(thing);
                        }
                    }
                }
                if (thing is { id: "plank", material.alias: "rosewood", IsImportant: false })
                {
                    if (plankCountNeeded > 0)
                    {
                        if (thing.Num > plankCountNeeded)
                        {
                            thing.Num -= plankCountNeeded;
                        }
                        else
                        {
                            plankCountNeeded -= thing.Num;
                            EClass.pc.things.Remove(thing);
                            EClass.pc.things.OnRemove(thing);
                        }
                    }
                }
                if (thing is { id: "crystal_sun", IsImportant: false })
                {
                    if (crystalCountNeeded > 0)
                    {
                        if (thing.Num > crystalCountNeeded)
                        {
                            thing.Num -= crystalCountNeeded;
                        }
                        else
                        {
                            crystalCountNeeded -= thing.Num;
                            EClass.pc.things.Remove(thing);
                            EClass.pc.things.OnRemove(thing);
                        }
                    }
                }
            }

            return true;
        }

        return false;
    }

    static bool check_harp_repair_materials(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        // Need 5 Dragonscale Thread, 5 Feywood Planks, 1 Mana Crystal

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

        // Search all storage for these items
        int threadCountNeeded = 5; // Need 5 Threads
        int plankCountNeeded = 5; // Need 5 Planks
        int crystalCountNeeded = 1; // Need 1 Mana Crystal
        int threadCount = 0;
        int plankCount = 0;
        int crystalCount = 0;
        foreach (Thing thing in allStorage)
        {
            if (thing is { id: "thread", material.alias: "hide_dragon", IsImportant: false })
            {
                threadCount += thing.Num;
            }
            if (thing is { id: "plank", material.alias: "feywood", IsImportant: false })
            {
                plankCount += thing.Num;
            }
            if (thing is { id: "crystal_mana", IsImportant: false })
            {
                crystalCount += thing.Num;
            }
        }

        if (threadCount >= 5 && plankCount >= 5 && crystalCount >= 1)
        {
            // Go back through and remove the designated amounts.
            foreach (Thing thing in allStorage)
            {
                if (thing is { id: "thread", material.alias: "hide_dragon", IsImportant: false })
                {
                    if (threadCountNeeded > 0)
                    {
                        if (thing.Num > threadCountNeeded)
                        {
                            thing.Num -= threadCountNeeded;
                        }
                        else
                        {
                            threadCountNeeded -= thing.Num;
                            EClass.pc.things.Remove(thing);
                            EClass.pc.things.OnRemove(thing);
                        }
                    }
                }
                if (thing is { id: "plank", material.alias: "feywood", IsImportant: false })
                {
                    if (plankCountNeeded > 0)
                    {
                        if (thing.Num > plankCountNeeded)
                        {
                            thing.Num -= plankCountNeeded;
                        }
                        else
                        {
                            plankCountNeeded -= thing.Num;
                            EClass.pc.things.Remove(thing);
                            EClass.pc.things.OnRemove(thing);
                        }
                    }
                }
                if (thing is { id: "crystal_mana", IsImportant: false })
                {
                    if (crystalCountNeeded > 0)
                    {
                        if (thing.Num > crystalCountNeeded)
                        {
                            thing.Num -= crystalCountNeeded;
                        }
                        else
                        {
                            crystalCountNeeded -= thing.Num;
                            EClass.pc.things.Remove(thing);
                            EClass.pc.things.OnRemove(thing);
                        }
                    }
                }
            }

            return true;
        }

        return false;
    }

    static bool niyon_not_in_party(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        Chara niyon = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.NiyonCharaId);
        return !(niyon.IsPCParty);
    }
    
    static bool niyon_in_party(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        Chara niyon = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.NiyonCharaId);
        return (niyon.IsPCParty);
    }

    static bool check_selena_awakening(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        EClass.player.dialogFlags.TryGetValue("selenaAwakeningYowyn", out int selenaAwakeningYowyn);
        EClass.player.dialogFlags.TryGetValue("selenaAwakeningBeach", out int selenaAwakeningBeach);
        EClass.player.dialogFlags.TryGetValue("selenaAwakeningMifu", out int selenaAwakeningMifu);
        return (selenaAwakeningYowyn == 1 && selenaAwakeningBeach == 1 && selenaAwakeningMifu == 1);
    }

    static bool niyon_awaken(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        Chara niyon = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.NiyonCharaId);
        if (niyon is not null)
        {
            EClass.player.dialogFlags["niyonAwakened"] = 1;
            EClass.player.dialogFlags["niyonAwakeningInProgress"] = 0;
            EClass.pc.Say("niyonAwakening".langGame());
            niyon.SetFeat(Constants.FeatMysticMusician, 1);            
        }

        return true;
    }

    static bool selena_awaken(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        Chara selena = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.SelenaCharaId);
        if (selena is not null)
        {
            EClass.player.dialogFlags["selenaAwakened"] = 1;
            EClass.player.dialogFlags["selenaAwakeningInProgress"] = 0;
            EClass.pc.Say("selenaAwakening".langGame());
            selena.SetFeat(Constants.FeatTimelessSong, 1);
        }

        return true;
    }

    static bool bard_awaken(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        EClass.pc.Say("playerBardAwakening".langGame());
        EClass.pc.SetFeat(Constants.FeatBardId, 1);
        return true;
    }
    
    static bool restock_niyon(DramaManager dm, Dictionary<string, string> line, params string[] parameters)
    {
        Chara niyon = game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.NiyonCharaId);
        if (niyon is not null)
        {
            niyon.c_dateStockExpire = 0;
        }
        return true;
    }
}