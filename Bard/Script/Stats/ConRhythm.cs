using System;
using System.Collections.Generic;
using BardMod.Common;
using Newtonsoft.Json;
using UnityEngine;
namespace BardMod.Stats;

public class ConRhythm : Timebuff
{

    [JsonProperty(PropertyName = "S")]
    private int Stacks = 1;

    [JsonProperty(PropertyName = "L")]
    public Constants.BardSongType LastPlayedSong;

    [JsonProperty(PropertyName = "M")]
    public Constants.BardMotif Motif = Constants.BardMotif.None;

    public override bool WillOverride => true;

    public override string TextDuration => "" + Stacks;
    public override bool CanStack(Condition c)
    {
        return true;
    }

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void OnStart()
    {
        Stacks = 0;
        LastPlayedSong = Constants.BardSongType.None;
        base.OnStart();
    }

    public void ModStacks(int value)
    {
        Stacks += value;
        Stacks = Math.Min(30, Stacks);

        if (Stacks <= 0)
        {
            Kill();
        }
    }

    public int GetStacks()
    {
        return Stacks;
    }

    public void Refresh()
    {
        // Caps at 60 turns
        Mod(Math.Max(30 - value, 0));
    }

    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintRhythm".lang(GetStacks().ToString(), GetStacks() > 1 ? "s" : ""));
        if (LastPlayedSong is Constants.BardSongType.Chorus) list.Add("hintChorus".lang());
        if (LastPlayedSong is Constants.BardSongType.Verse) list.Add("hintVerse".lang());
        if (Stacks >= 10)
        {
            list.Add("hintRhythmFinale".lang(Motif.ToString()));
        }
    }
}