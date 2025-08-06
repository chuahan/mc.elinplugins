using System.Collections.Generic;
using BardMod.Common;
namespace BardMod.Stats.BardSongConditions;

public class ConPrismaticBridgeSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;

    public override void OnWriteNote(List<string> list)
    {
        string plural = Stacks is > 1 or 0 ? "s" : "";
        list.Add("hintPrismaticBridgeSong".lang(Stacks.ToString(), plural));
    }
}