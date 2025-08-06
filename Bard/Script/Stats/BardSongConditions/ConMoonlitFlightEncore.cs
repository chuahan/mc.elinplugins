using BardMod.Common;
namespace BardMod.Stats.BardSongConditions;

// Encore Songs: Some Songs might have an after-effect.
// This one in particular is if Moonlit Flight explodes onto an ally to the original caster.
// They will gain a temporary boost of stats.
public class ConMoonlitFlightEncore : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;
}