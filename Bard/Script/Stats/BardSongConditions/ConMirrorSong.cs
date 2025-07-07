using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

public class ConMirrorSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
    public override ConditionType Type => ConditionType.Buff;
}