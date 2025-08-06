using System;
using BardMod.Common;
namespace BardMod.Stats.BardSongConditions;

public class ConEphemeralFlowersSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Debuff;

    public int GetHpPercentDamage()
    {
        return Math.Min(10, RhythmStacks);
    }
}