using BardMod.Common;
namespace BardMod.Stats.BardSongConditions;

// Purely a stat buff song.
public class ConSpeedSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
    public override ConditionType Type => ConditionType.Buff;
}