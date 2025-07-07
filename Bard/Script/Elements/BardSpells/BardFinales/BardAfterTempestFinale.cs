using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells.BardFinales;

public class BardAfterTempestFinale: BardSongData
{
    public override string SongName => Constants.BardFinaleAfterTempestName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Self;

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConAfterTempestSong bardBuff = ConBardSong.Create(nameof(ConAfterTempestSong), power, rhythmStacks, godBlessed, bard) as ConAfterTempestSong;
        target.AddCondition(bardBuff);
    }
}