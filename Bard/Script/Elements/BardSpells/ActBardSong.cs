using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats;
using UnityEngine;

namespace BardMod.Elements.BardSpells;

public abstract class ActBardSong : AIAct
{
	protected abstract BardSongData SongData { get; }
    private SoundSource soundSource;
    private Thing tool;
    public override bool ShowProgress => false;
    public override bool CancelWhenDamaged => false;
    public int turnsElapsed = 0;

    private static readonly IReadOnlyDictionary<int, Constants.BardMotif> MotifMap = new Dictionary<int, Constants.BardMotif>
    {
	    { Constants.BardWindSongEnc, Constants.BardMotif.Wind },
	    { Constants.BardWaterSongEnc, Constants.BardMotif.Water },
	    { Constants.BardLightningSongEnc, Constants.BardMotif.Lightning },
	    { Constants.BardFlameSongEnc, Constants.BardMotif.Flame },
	    { Constants.BardBlossomSongEnc, Constants.BardMotif.Flower },
	    { Constants.BardImpactSongEnc, Constants.BardMotif.Vibration },
	    { Constants.BardLightSongEnc, Constants.BardMotif.Light },
	    { Constants.BardDarkSongEnc, Constants.BardMotif.Darkness },
	    { Constants.BardTravelerSongEnc, Constants.BardMotif.Eternalism },
	    { Constants.BardEternalEnc, Constants.BardMotif.Ethereal },
	    { Constants.BardRevenantEnc, Constants.BardMotif.Revenant },
	    { Constants.BardTempestEnc, Constants.BardMotif.Tempest },
	    { Constants.BardCapriccioEnc, Constants.BardMotif.Starry },
	    { Constants.BardPianissimoEnc, Constants.BardMotif.Ephemeral },
	    { Constants.BardSolacetuneEnc, Constants.BardMotif.Moonchill }
    };
    
    public override Cost GetCost(Chara c)
    {
        Act.Cost result2 = default(Act.Cost);
        result2.type = Act.CostType.MP;
        
        int num = EClass.curve(Value, 50, 10);
        result2.cost = source.cost[0] * (100 + ((!source.tag.Contains("noCostInc")) ? (num * 3) : 0)) / 100;
        
        // Higher Music skill will reduce mana costs.
        if (c != null)
        {
	        int musicSkill = c.Chara.Evalue(Constants.MusicSkill);
	        result2.cost *= (100 / (100 + musicSkill));
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

	public override bool CanPerform()
	{
		if (!CC.IsPC)
		{
			if (CC.HasCooldown(SongData.SongId))
			{
				return false;
			}
		}
		
		return base.CanPerform();
	}

	public override bool Perform()
	{
		if (owner != null && !owner.IsPC)
		{
			EClass.pc.Say(owner.Name + " performs " + SongData.SongName);
			DoBardMagic();
			owner.AddCooldown(SongData.SongId, 5);
			return true;
		}
		
		return base.Perform();
	}
			
	public override IEnumerable<Status> Run()
	{
		if (BardMod.Debug) owner.Say("Turn Start " + turnsElapsed);
		if (owner == null)
		{
			yield return Cancel();
		}

		// NPC Bard Actions are different, they fire off instantly and instead have a cooldown of 5 turns. This should be handled in Perform though.
		if (owner != null && !owner.IsPC)
		{
			yield return Status.Success;
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
			owner.Say("noBardInstrumentEquipped".langGame());
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
				owner.ShowEmo(Emo.happy, 3f, skipSame: false);
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
			onProgress = delegate(Progress_Custom p)
			{
				turnsElapsed++;
				if (BardMod.Debug) owner.Say("Turn Progress " + turnsElapsed);
				foreach (Chara item in EClass._map.ListCharasInCircle(owner.pos, SongData.SongRadius))
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
						owner.elements.ModExp(Constants.MusicSkill, EClass._zone.IsUserZone ? 10 : 50);
					}
				}
			},
			onProgressComplete = delegate
			{
				if (BardMod.Debug) owner.Say("Turn End " + turnsElapsed);
				if (soundSource && soundSource.isPlaying) soundSource.Stop();
				
				if (owner == null)
				{
					return;
				}
				
				this.DoBardMagic();
			}
		}.SetDuration(SongData.SongLength);
		yield return Do(seq);
	}

	public void DoBardMagic()
	{
		// Rhythm affects some songs
		ConRhythm rhythm = owner.GetCondition<ConRhythm>();
		if (rhythm == null)
		{
			rhythm = owner.AddCondition<ConRhythm>() as ConRhythm;
		}

		bool onRhythm = this.UpdateRhythm(rhythm);
		
		// If an ally has the Duet Feat, triple Rhythm gain and increase effects.
		bool isDuet = false;
		if (owner.IsPCParty || owner.IsPC)
		{
			foreach (Chara partyMember in pc.party.members)
			{
				if (partyMember != owner && partyMember.Evalue(Constants.FeatDuetPartner) > 0)
				{
					isDuet = true;
				}
			}
		}
		
		// For Debugging, max out your Rhythm
		if (BardMod.Debug) rhythm.ModStacks(30);
		
		// Get the motif trait from the instrument and add it to the Rhythm if PC.
		if (owner.IsPC)
		{
			foreach (var key in MotifMap.Keys)
			{
				if (tool.elements.dict.ContainsKey(key))
				{
					rhythm.Motif = MotifMap[key];
					break;
				}	
			}
		}

		// Some songs have an advanced effect for specific religions.
		bool godBlessed = SongData.AffiliatedReligion != null && owner.faith.id == SongData.AffiliatedReligion;
		
		// Prepare to apply Song Effects
		float radius = SongData.SongRadius;
		if (owner.Evalue(Constants.FeatSoulsingerId) > 0)
		{
			radius *= 2;
		}
		
		(List<Chara> friendlyTargets, List<Chara> enemyTargets) = SortAffected(owner, (int)radius);
		int songPower = GetPower(owner);
		int rhythmStacks = rhythm.GetStacks();

		// If it is a finale.
		// Say the lines
		// Scale power based on Rhythm.
		if (SongData.SongType is Constants.BardSongType.Finale)
		{
			string performQuote = "perform_" + SongData.SongName;
			string finaleProperName = "finale_" + SongData.SongName;
			owner.Say("perform_finale".langGame(owner.NameSimple, finaleProperName.langGame()));
			owner.TalkRaw(performQuote.langGame());
			
			int rhythmPower = rhythmStacks / 10;
			float rhythmPowerFactor = rhythmPower / 3F;
			songPower = (int)(songPower * rhythmPowerFactor);
		}
		
		// Play Song SFX and FX if needed.
		SongData.PlayEffects(owner);
		
		// Apply Song Effects to targets.
		switch (SongData.SongTarget)
		{
			case Constants.BardSongTarget.Self:
				owner.elements.ModExp(SongData.SongId, 50);
				SongData.ApplyFriendlyEffect(owner, owner, songPower, rhythmStacks, godBlessed);
				break;
			case Constants.BardSongTarget.Both:
				foreach (Chara target in friendlyTargets)
				{
					owner.elements.ModExp(SongData.SongId, 20);
					SongData.ApplyFriendlyEffect(owner, target, songPower, rhythmStacks, godBlessed);
				
				}
				foreach (Chara target in enemyTargets)
				{
					owner.elements.ModExp(SongData.SongId, 20);
					SongData.ApplyEnemyEffect(owner, target, songPower, rhythmStacks, godBlessed);
				
				}
				break;
			case Constants.BardSongTarget.Friendly:
				foreach (Chara target in friendlyTargets)
				{
					owner.elements.ModExp(SongData.SongId, 20);
					SongData.ApplyFriendlyEffect(owner, target, songPower, rhythmStacks, godBlessed);
				}
				break;
			case Constants.BardSongTarget.Enemy:
				foreach (Chara target in enemyTargets)
				{
					owner.elements.ModExp(SongData.SongId, 20);
					SongData.ApplyEnemyEffect(owner, target, songPower, rhythmStacks, godBlessed);
				}
				break;
		}
		
		bool isBard = owner.Evalue(Constants.FeatBardId) > 0;
		if (SongData.SongType is Constants.BardSongType.Finale)
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
		if (BardMod.Debug) owner.Say("Turn End " + turnsElapsed);
		if (soundSource != null)
		{
			soundSource.Stop();
		}
	}

	public (List<Chara>, List<Chara>) SortAffected(Chara musician, int radius)
	{
		List<Chara> affectedAllies = new List<Chara>();
		List<Chara> affectedEnemies = new List<Chara>();
		foreach (Point item in musician.currentZone.map.ListPointsInCircle(musician.pos, radius, mustBeWalkable: false, los:false))
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
		return (affectedAllies,affectedEnemies);
	}
	
	public bool UpdateRhythm(ConRhythm rhythm)
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
				if (SongData.SongType is Constants.BardSongType.Chorus ||
				    SongData.SongType is Constants.BardSongType.Finale) followsRhythm = true;
				break;
			case Constants.BardSongType.Chorus:
				if (SongData.SongType is Constants.BardSongType.Verse ||
				    SongData.SongType is Constants.BardSongType.Finale) followsRhythm = true;
				break;
		}

		rhythm.LastPlayedSong = SongData.SongType;
		return followsRhythm;
	}
	
	public override int GetPower(Card bard)
	{
		// Get Base Power.
		int basePower = base.Value * 8 + 50;
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
		if (!this.SongData.IsNull())
		{
			int levelPower = 10 * (100 + (bard.Evalue(SongData.SongId) - 1) * 10) / 100;
			basePower += levelPower;
		}
		
		// Extra Multipliers
		float powerMultiplier = 1f;
		
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
		basePower = HelperFunctions.SafeMultiplier(basePower,powerMultiplier);
		
		if (BardMod.Debug) owner.Say("BardPower Precurve" + basePower);
		
		// Curve cause reasons.
		basePower = EClass.curve(basePower, 500, 100);
        
		if (BardMod.Debug) owner.Say("BardPower Postcurve" + basePower);
		return basePower;
	}
}