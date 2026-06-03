using BardMod.Common;
namespace BardMod.Stats;

// Purely a Stat Debuff Song.
public class ConWitheringSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
    public override ConditionType Type => ConditionType.Debuff;
}