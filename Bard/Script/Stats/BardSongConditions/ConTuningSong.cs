using BardMod.Common;
namespace BardMod.Stats.BardSongConditions;

// Stat debuff
public class ConTuningSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
    public override ConditionType Type => ConditionType.Debuff;
}