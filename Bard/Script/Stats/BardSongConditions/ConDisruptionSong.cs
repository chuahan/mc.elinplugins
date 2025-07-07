using BardMod.Common;

namespace BardMod.Stats.BardSongConditions;

// Purely a stat debuff song.
public class ConDisruptionSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
    public override ConditionType Type => ConditionType.Debuff;
}