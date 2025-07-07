using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

// Stat debuff
public class ConTuningSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
    public override ConditionType Type => ConditionType.Debuff;
}