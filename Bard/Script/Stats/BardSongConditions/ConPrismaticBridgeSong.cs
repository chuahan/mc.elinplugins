using System;
using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

/*
 * All allies gain elemental resistances across the board.
 * Grants up to 5 charges of Charged Strikes which double 5 instances of outgoing damage.
 */
public class ConPrismaticBridgeSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;

    public int Stacks = 0;

    public override void OnWriteNote(List<string> list)
    {
        string plural = this.Stacks is > 1 or 0 ? "s" : "";
        list.Add("hintPrismaticBridgeSong".lang(this.Stacks.ToString(), plural));
    }
}