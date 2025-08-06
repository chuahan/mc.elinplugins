using BardMod.Common;
namespace BardMod.Stats.BardSongConditions;

// Purely a Stat Debuff Song.
public class ConWitheringSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
    public override ConditionType Type => ConditionType.Debuff;
}