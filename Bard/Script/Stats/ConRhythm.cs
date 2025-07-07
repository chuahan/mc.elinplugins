using System;
using System.Collections.Generic;
using BardMod.Common;
using UnityEngine;

namespace BardMod.Stats;

public class ConRhythm : Timebuff
{
    public override bool WillOverride => true;
    public override bool CanStack(Condition c)
    {
        return true;
    }

    private int _stacks = 1;
    public Constants.BardSongType LastPlayedSong;
    public Constants.BardMotif Motif = Constants.BardMotif.None;
    
    public override string TextDuration => "" + _stacks;
    
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public override void OnStart()
    {
        _stacks = 0;
        LastPlayedSong = Constants.BardSongType.None;
        base.OnStart();
    }

    public void ModStacks(int value)
    {
        _stacks += value;
        _stacks = Math.Min(30, _stacks);
        
        if (_stacks <= 0)
        {
            Kill();
        }
    }
    
    public int GetStacks()
    {
        return _stacks;
    }

    public void Refresh()
    {
        // Caps at 60 turns
        this.Mod(Math.Max(30 - this.value, 0));
    }
    
    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintRhythm".lang(GetStacks().ToString(), (GetStacks() > 1) ? "s" : ""));
        if (LastPlayedSong is Constants.BardSongType.Chorus) list.Add("hintChorus".lang());
        if (LastPlayedSong is Constants.BardSongType.Verse) list.Add("hintVerse".lang());
        if (_stacks >= 10)
        {
            list.Add("hintRhythmFinale".lang(Motif.ToString()));
        }
    }
}