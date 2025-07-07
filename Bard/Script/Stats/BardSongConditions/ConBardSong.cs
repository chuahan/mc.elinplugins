using System;
using BardMod.Common;
using UnityEngine;

namespace BardMod.Stats.BardSongConditions;

public class ConBardSong : Timebuff
{
    public override bool CanManualRemove => false;
    public override bool WillOverride => true;
    
    public bool GodBlessed = false;

    protected int RhythmStacks = 0;

    public Chara Caster;
    
    public override int P2 => Math.Max(1, RhythmStacks / 10);
    
    public static ConBardSong Create(string alias, int power, int rhythm, bool godblessed, Chara caster, Action<Condition> onCreate = null)
    {
        SourceStat.Row row = EClass.sources.stats.alias[alias];
        ConBardSong condition = ClassCache.Create<ConBardSong>(row.type.IsEmpty(alias), "Elin");
        condition.power = power;
        condition.RhythmStacks = rhythm;
        condition.GodBlessed = godblessed;
        condition.Caster = caster;
        condition.id = row.id;
        condition._source = row;
        onCreate?.Invoke(condition);
        return condition;
    }
    
    public void RefreshBardSong()
    {
        if (this.SongType == Constants.BardSongType.Finale) return;
        this.Mod(Mathf.Max(30 - this.value, 0)); // Basic Songs go up to 30 turns.
    }
    
    public virtual Constants.BardSongType SongType { get; }
}