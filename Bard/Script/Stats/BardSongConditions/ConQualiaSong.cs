using BardMod.Common;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Stats;

// Purely a stat buff song.
public class ConQualiaSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
    public override ConditionType Type => ConditionType.Buff;
}