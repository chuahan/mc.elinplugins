using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats;
using BardMod.Traits;
using UnityEngine;
namespace BardMod.Elements.BardSpells;

public abstract class ActBardSong : AIAct
{

    private static readonly IReadOnlyDictionary<int, Constants.BardMotif> MotifMap = new Dictionary<int, Constants.BardMotif>
    {
        {
            Constants.BardWindSongEnc, Constants.BardMotif.Wind
        },
        {
            Constants.BardWaterSongEnc, Constants.BardMotif.Water
        },
        {
            Constants.BardLightningSongEnc, Constants.BardMotif.Lightning
        },
        {
            Constants.BardFlameSongEnc, Constants.BardMotif.Flame
        },
        {
            Constants.BardBlossomSongEnc, Constants.BardMotif.Flower
        },
        {
            Constants.BardImpactSongEnc, Constants.BardMotif.Vibration
        },
        {
            Constants.BardLightSongEnc, Constants.BardMotif.Light
        },
        {
            Constants.BardDarkSongEnc, Constants.BardMotif.Darkness
        },
        {
            Constants.BardTravelerSongEnc, Constants.BardMotif.Eternalism
        },
        {
            Constants.BardEternalEnc, Constants.BardMotif.Ethereal
        },
        {
            Constants.BardRevenantEnc, Constants.BardMotif.Revenant
        },
        {
            Constants.BardTempestEnc, Constants.BardMotif.Tempest
        },
        {
            Constants.BardCapriccioEnc, Constants.BardMotif.Starry
        },
        {
            Constants.BardPianissimoEnc, Constants.BardMotif.Ephemeral
        },
        {
            Constants.BardSolacetuneEnc, Constants.BardMotif.Moonchill
        }
    };

    private SoundSource soundSource;
    private Thing tool;
    public int turnsElapsed;
    protected abstract BardSongData SongData { get; }
    public override bool ShowProgress => false;
    public override bool CancelWhenDamaged => false;

    public override Cost GetCost(Chara c)
    {
        Cost result2 = default(Cost);
        result2.type = CostType.MP;

        int num = EClass.curve(Value, 50, 10);
        result2.cost = source.cost[0] * (100 + (!source.tag.Contains("noCostInc") ? num * 3 : 0)) / 100;

        // Higher Music skill will reduce mana costs.
        if (c != null)
        {
            int musicSkill = c.Chara.Evalue(Constants.MusicSkill);
            result2.cost *= 100 / (100 + musicSkill);
        }

        if ((c == null || !c.IsPC) && result2.cost > 2)
        {
            result2.cost /= 2;
        }

        return result2;
    }

    public override bool CanManualCancel()
    {
        return true;
    }

    public override IEnumerable<Status> Run()
    {
        if (owner == null)
        {
            yield return Cancel();
        }

        // This path is for PC only.
        if (!owner.IsPC)
        {
            yield return Cancel();
        }

        // Get Active Instrument
        List<Thing> allInstruments = HelperFunctions.GetAllInstruments();
        foreach (Thing instrument in allInstruments)
        {
            if ((instrument.trait as TraitToolBard).IsSelectedInstrument)
            {
                tool = instrument;
            }
        }

        // If no tool was found, default to the first one.
        if (tool == null) tool = owner.things.Find<TraitToolBard>();

        // Hold instrument for performance.
        if (tool != null)
        {
            owner.HoldCard(tool, 1);
            yield return DoGrab(tool, 1);
        }

        if (tool == null)
        {
            Msg.Say("noBardInstrumentEquipped".langGame());
            yield return Cancel();
        }

        List<Chara> reacted = new List<Chara>();
        Progress_Custom seq = new Progress_Custom
        {
            maxProgress = 15,
            cancelWhenMoved = true,
            showProgress = true,
            canProgress = () => tool != null && !tool.isDestroyed,
            onProgressBegin = delegate
            {
                owner.Say("music_start", owner, tool);
                owner.ShowEmo(Emo.happy, 3f, false);
                if (tool != null && tool.ExistsOnMap)
                {
                    owner.LookAt(tool);
                }

                owner.PlayAnime(AnimeID.PlayMusic);
                if (owner.IsPC && BardConfig.UseMusic?.Value == true)
                {
                    soundSource = owner.PlaySound(SongData.SongName);
                }
            },
            onProgress = delegate
            {
                turnsElapsed++;
                foreach (Chara item in _map.ListCharasInCircle(owner.pos, SongData.SongRadius))
                {
                    if (item.conSleep != null && item.ResistLv(957) <= 0)
                    {
                        item.conSleep.Kill();
                        item.ShowEmo(Emo.angry);
                    }
                }

                List<Chara> list = owner.pos.ListWitnesses(owner, (int)SongData.SongRadius, WitnessType.music);
                foreach (Chara item2 in list)
                {
                    if (owner == null)
                    {
                        break;
                    }

                    if (!reacted.Contains(item2))
                    {
                        reacted.Add(item2);
                        // Add Music EXP.
                        owner.elements.ModExp(Constants.MusicSkill, _zone.IsUserZone ? 10 : 50);
                    }
                }
            },
            onProgressComplete = delegate
            {
                if (soundSource && soundSource.isPlaying) soundSource.Stop();

                if (owner == null)
                {
                    return;
                }

                int bardPower = GetPower(owner);
                ActBardSong.DoBardMagic(owner, SongData, bardPower, tool);
            }
        }.SetDuration(SongData.SongLength);
        yield return Do(seq);
    }

    public static void DoBardMagic(Chara bard, BardSongData songData, int bardPower, Thing tool = null)
    {
        // Rhythm affects some songs
        ConRhythm rhythm = bard.GetCondition<ConRhythm>();
        if (rhythm == null)
        {
            rhythm = bard.AddCondition<ConRhythm>() as ConRhythm;
        }

        bool onRhythm = ActBardSong.UpdateRhythm(rhythm, songData);

        // If an ally has the Duet Feat, triple Rhythm gain and increase effects.
        bool isDuet = false;
        if (bard.IsPCParty || bard.IsPC)
        {
            foreach (Chara partyMember in pc.party.members)
            {
                if (partyMember != bard && partyMember.Evalue(Constants.FeatDuetPartner) > 0)
                {
                    isDuet = true;
                }
            }
        }

        // For Debugging, max out your Rhythm
        if (BardMod.Debug) rhythm.ModStacks(30);

        // Get the motif trait from the instrument and add it to the Rhythm if PC.
        if (bard.IsPC && tool != null)
        {
            foreach (int key in MotifMap.Keys)
            {
                if (tool.elements.dict.ContainsKey(key))
                {
                    rhythm.Motif = MotifMap[key];
                    break;
                }
            }
        }

        // Some songs have an advanced effect for specific religions.
        bool godBlessed = songData.AffiliatedReligion != null && bard.faith.id == songData.AffiliatedReligion;
        if (godBlessed) Msg.Say("hintGodBlessedSong".langGame());

        // Prepare to apply Song Effects
        float radius = songData.SongRadius;
        if (bard.Evalue(Constants.FeatSoulsingerId) > 0)
        {
            radius *= 2;
        }

        (List<Chara> friendlyTargets, List<Chara> enemyTargets) = ActBardSong.SortAffected(bard, (int)radius);
        int rhythmStacks = rhythm.GetStacks();

        // If it is a finale.
        // Say the lines
        // Scale power based on Rhythm.
        if (songData.SongType is Constants.BardSongType.Finale)
        {
            string performQuote = "perform_" + songData.SongName;
            string finaleProperName = "finale_" + songData.SongName;
            Msg.Say("perform_finale".langGame(bard.NameSimple, finaleProperName.langGame()));
            bard.TalkRaw(performQuote.langGame());

            int rhythmPower = rhythmStacks / 10;
            float rhythmPowerFactor = rhythmPower / 3F;
            bardPower = (int)(bardPower * rhythmPowerFactor);
        }

        // Play Song SFX and FX if needed.
        songData.PlayEffects(bard);

        // Apply Song Effects to targets.
        switch (songData.SongTarget)
        {
            case Constants.BardSongTarget.Self:
                bard.elements.ModExp(songData.SongId, 50);
                songData.ApplyFriendlyEffect(bard, bard, bardPower, rhythmStacks, godBlessed);
                break;
            case Constants.BardSongTarget.Both:
                foreach (Chara target in friendlyTargets)
                {
                    bard.elements.ModExp(songData.SongId, 20);
                    songData.ApplyFriendlyEffect(bard, target, bardPower, rhythmStacks, godBlessed);

                }
                foreach (Chara target in enemyTargets)
                {
                    bard.elements.ModExp(songData.SongId, 20);
                    songData.ApplyEnemyEffect(bard, target, bardPower, rhythmStacks, godBlessed);

                }
                break;
            case Constants.BardSongTarget.Friendly:
                foreach (Chara target in friendlyTargets)
                {
                    bard.elements.ModExp(songData.SongId, 20);
                    songData.ApplyFriendlyEffect(bard, target, bardPower, rhythmStacks, godBlessed);
                }
                break;
            case Constants.BardSongTarget.Enemy:
                foreach (Chara target in enemyTargets)
                {
                    bard.elements.ModExp(songData.SongId, 20);
                    songData.ApplyEnemyEffect(bard, target, bardPower, rhythmStacks, godBlessed);
                }
                break;
        }

        bool isBard = bard.Evalue(Constants.FeatBardId) > 0;
        if (songData.SongType is Constants.BardSongType.Finale)
        {
            if (isBard)
            {
                // Feat Bard will only make you lose half your stacks instead.
                int rhythmLoss = -1 * rhythm.GetStacks() / 2;
                rhythm.ModStacks(rhythmLoss);
                rhythm.Refresh();
            }
            else
            {
                rhythm.Kill();
            }
        }
        else
        {
            int rhythmMod = 1;
            if (onRhythm)
            {
                rhythmMod += 1;
                if (isBard) rhythmMod += 2;
                if (isDuet) rhythmMod += 2;
            }
            else
            {
                rhythmMod *= -1;
            }

            rhythm.ModStacks(rhythmMod);
            rhythm.Refresh();
        }
    }

    public override void OnCancel()
    {
        if (soundSource != null)
        {
            soundSource.Stop();
        }
    }

    public static (List<Chara>, List<Chara>) SortAffected(Chara musician, int radius)
    {
        List<Chara> affectedAllies = new List<Chara>();
        List<Chara> affectedEnemies = new List<Chara>();
        foreach (Point item in musician.currentZone.map.ListPointsInCircle(musician.pos, radius, false, false))
        {
            List<Chara> pointCharacters = item.detail?.charas;
            if (pointCharacters == null || pointCharacters.Count == 0)
            {
                continue;
            }

            foreach (Chara listener in pointCharacters)
            {
                if (!listener.IsHostile(musician))
                {
                    affectedAllies.Add(listener);
                }
                else
                {
                    affectedEnemies.Add(listener);
                }
            }
        }
        return (affectedAllies, affectedEnemies);
    }

    public static bool UpdateRhythm(ConRhythm rhythm, BardSongData songdata)
    {
        // None can be followed by Any.
        // Verse must be followed by Chorus or Finale.
        // Chorus must be followed by Verse or Finale.
        // Finale can be followed by Any.
        bool followsRhythm = false;
        switch (rhythm.LastPlayedSong)
        {
            case Constants.BardSongType.None:
            case Constants.BardSongType.Finale:
                followsRhythm = true;
                break;
            case Constants.BardSongType.Verse:
                if (songdata.SongType is Constants.BardSongType.Chorus ||
                    songdata.SongType is Constants.BardSongType.Finale) followsRhythm = true;
                break;
            case Constants.BardSongType.Chorus:
                if (songdata.SongType is Constants.BardSongType.Verse ||
                    songdata.SongType is Constants.BardSongType.Finale) followsRhythm = true;
                break;
        }

        rhythm.LastPlayedSong = songdata.SongType;
        return followsRhythm;
    }

    public override int GetPower(Card bard)
    {
        // Get Base Power.
        int basePower = Value * 8 + 50;
        int musicSkill = bard.Evalue(Constants.MusicSkill);
        int charisma = bard.Evalue(Constants.ChaAttribute);
        if (!bard.IsPC)
        {
            basePower = Mathf.Max(basePower, bard.LV * 6 + 30);
            basePower = Mathf.Max(basePower, charisma * 4 + 30);
            basePower += musicSkill;
        }
        // Add Music Skill
        basePower += musicSkill;
        // Add Charisma
        basePower += charisma;

        // Song Level added to base power.
        if (!SongData.IsNull())
        {
            int levelPower = 10 * (100 + (bard.Evalue(SongData.SongId) - 1) * 10) / 100;
            basePower += levelPower;
        }

        // Extra Multipliers
        float powerMultiplier = 1f;

        // Pianist Multiplier
        if (bard.c_idJob == "pianist") powerMultiplier += 0.5F;

        // Sweet Voice Mutation
        if (bard.Evalue(1522) > 0) powerMultiplier += 0.25f;
        // Husky Voice Mutation
        if (bard.Evalue(1523) > 0) powerMultiplier -= 0.25f;

        // Duet Multiplier
        if (bard.IsPCParty || bard.IsPC)
        {
            foreach (Chara partyMember in pc.party.members)
            {
                if (partyMember != owner && partyMember.Evalue(Constants.FeatDuetPartner) > 0)
                {
                    powerMultiplier += 1;
                }
            }
        }

        // Bard Multiplier
        if (bard.Evalue(Constants.FeatBardId) > 0) powerMultiplier += 0.5f;

        // Apply Multipliers
        basePower = HelperFunctions.SafeMultiplier(basePower, powerMultiplier);

        // Curve cause reasons.
        basePower = EClass.curve(basePower, 500, 100);

        return basePower;
    }
}