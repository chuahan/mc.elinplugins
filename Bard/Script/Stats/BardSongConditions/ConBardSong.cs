using System;
using BardMod.Common;
using Newtonsoft.Json;
using UnityEngine;
namespace BardMod.Stats.BardSongConditions;

public class ConBardSong : Timebuff
{

    [JsonProperty(PropertyName = "C")]
    public Chara Caster;

    [JsonProperty(PropertyName = "G")]
    public bool GodBlessed;

    [JsonProperty(PropertyName = "R")]
    public int RhythmStacks;

    [JsonProperty(PropertyName = "S")]
    public int Stacks;

    public override bool CanManualRemove => false;
    public override bool WillOverride => true;

    public override int P2 => Math.Max(1, RhythmStacks / 10);

    public virtual Constants.BardSongType SongType { get; }

    public static ConBardSong Create(string alias, int power, int rhythm, bool godblessed, Chara caster, Action<Condition> onCreate = null)
    {
        SourceStat.Row row = sources.stats.alias[alias];
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
        if (SongType == Constants.BardSongType.Finale) return;
        Mod(Mathf.Max(30 - value, 0)); // Basic Songs go up to 30 turns.
    }
}